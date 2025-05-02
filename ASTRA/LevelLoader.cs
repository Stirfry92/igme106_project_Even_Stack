using ASTRA.Scenes;
using ASTRA.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA
{
    internal class LevelLoader
    {


        private GameObject[,] level;
        private StreamReader reader;
        private string name;
        private string nextLevel;
        private int LevelNum;
        private int Timer;
        private int x;
        private int y;
        private GameObjectDelegate Add;
        private GameObjectDelegate Remove;
        private Player player = null;
        public Player Player { get { return player; } }

        internal SetSceneDelegate OnNoNextLevel;

        private Vector2 PlayerPosition;

        public event Action<Rectangle> playerLocation;
        public event Action reset;
        
        internal LevelLoader()
        {
        }

        
        
        
        /// <summary>
        /// draws level from level array
        /// </summary>
        public void Update(GameTime gt)
        {
            playerLocation.Invoke(player.CollisionBounds);
            foreach (var item in level)
            {
                if (item != null)
                {
                    item.Update(gt);
                }
            }
        }


        



        /// <summary>
        /// loops through level array and draws
        /// </summary>
        public void DrawLevel(SpriteBatch asd)
        {
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    if (level[j, i] != null && level[j, i] is IDrawable drawable)
                    {
                        drawable.Draw(asd);
                    }

                }
            }
        }

        private void LoadLocal(string file)
        {
            Char[] asd;
            reader = new StreamReader(file);
            string[] data = reader.ReadLine().Split(',');
            name = data[0];
            LevelNum = int.Parse(data[1]);
            //Timer = int.Parse(data[2]);
            y = int.Parse(data[3]);
            x = int.Parse(data[4]);
            level = new GameObject[x, y];
            // sets up the level
            for (int i = 0; i < y; i++)
            {
                asd = reader.ReadLine().ToCharArray();
                for (int j = 0; j < x; j++)
                {
                    switch (asd[j])
                    {
                        case 'X'://wall
                            level[j, i] = new CollidableWall(new Vector2(j * GameDetails.TileSize, i * GameDetails.TileSize), new Vector2(GameDetails.TileSize, GameDetails.TileSize), "tile");
                            break;
                        case '!'://hole
                            //NOTE FROM STERLING: Commented out to represent actual holes.
                            //level[j, i] = new CollidableWall(new Vector2(j * GameDetails.TileSize, i * GameDetails.TileSize), new Vector2(GameDetails.TileSize, GameDetails.TileSize), "blank");

                            // needs hole class
                            break;
                        case 'B'://button
                            level[j, i] = new GameButton(new Vector2(j * GameDetails.TileSize, i * GameDetails.TileSize), new Vector2(GameDetails.TileSize, GameDetails.TileSize), "playerButton");
                            break;
                        case 'D'://closed door
                            level[j, i] = new GameDoor(new Vector2(j * GameDetails.TileSize, i * GameDetails.TileSize), new Vector2(GameDetails.TileSize, GameDetails.TileSize), "Wall1");
                            break;
                        case 'd'://opened door

                            break;
                        case 'O'://obstacle
                            level[j, i] = new CollidableWall(new Vector2(j * GameDetails.TileSize, i * GameDetails.TileSize), new Vector2(GameDetails.TileSize, GameDetails.TileSize), "Barrel");
                            break;
                        case '-'://open space
                            level[j, i] = new Floor(new Vector2(j * GameDetails.TileSize, i * GameDetails.TileSize), new Vector2(GameDetails.TileSize, GameDetails.TileSize), "panel");
                            break;
                        case 'E'://end
                            level[j, i] = new GameButton(new Vector2(j * GameDetails.TileSize, i * GameDetails.TileSize), new Vector2(GameDetails.TileSize, GameDetails.TileSize), "brokenPanel");
                            if (level[j, i] is GameButton ending)
                            {
                                ending.IsPressed += newLevel;
                                playerLocation += ending.CollidesWithPlayer;
                            }
                            break;
                        case 'S'://start
                            level[j, i] = new Floor(new Vector2(j * GameDetails.TileSize, i * GameDetails.TileSize), new Vector2(GameDetails.TileSize, GameDetails.TileSize), "panel");
                            PlayerPosition = new Vector2(j * GameDetails.TileSize, i * GameDetails.TileSize);
                            break;
                    }

                    Add(level[j, i]);
                }
            }
            // sets up the level logic
            data = reader.ReadLine().Split('+');
            do
            {
                logic(data[0], data[1]);
                data = reader.ReadLine().Split('+');
            } while (data[0] != "//");

            nextLevel = reader.ReadLine().Trim();
            reader.Close();

            if (player == null)
            {
                player = new Player(PlayerPosition);
                player.AddToParent = Add;
                player.RemoveFromParent = Remove;
            }
            else
            {
                player.Position = PlayerPosition;
            }

            Add(player);
            
        }


        //level loading section
        /// <summary>
        /// loads a single level file
        /// </summary>
        public void LoadLevel(string file, GameObjectDelegate add, GameObjectDelegate delete)
        {
            Add = add;
            Remove = delete;
            this.OnNoNextLevel = OnNoNextLevel;
            LoadLocal(file);
        }

        public void button(object a, EventArgs e)
        {
            playerLocation.Invoke(player.CollisionBounds);
        }



        /// <summary>
        /// connects a button with doors (not done needs buttons)
        /// </summary>
        public void logic(string buttonSet, string doorSet)
        {
            string[] Button = buttonSet.Split(',');
            int buttonX = int.Parse(Button[0]);
            int buttonY = int.Parse(Button[1]);   
            Button = doorSet.Split(',');

            int doorX = int.Parse(Button[0]);
            int doorY = int.Parse(Button[1]);

            if (level[buttonX, buttonY] is GameButton a && level[doorX, doorY] is GameDoor d)
            {
                playerLocation += a.CollidesWithPlayer;
                a.IsPressed += d.OpenDoor;
            }
        }

        internal void ResetCurrentLevel()
        {
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    level[j,i]?.Reset();
                }
            }

            Player.Position = PlayerPosition;
        }

        public void newLevel(object a, EventArgs e)
        {
            // removes all the gameobjects in level
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    if (level[j, i] != null) 
                    {
                        
                        if (level[j, i] is GameButton ending)
                        {
                            ending.IsPressed -= newLevel;
                            playerLocation -= ending.CollidesWithPlayer;
                        }
                        Remove(level[j, i]);
                    }
                    
                }
            }


            Remove(player);
            // resets hammers 
            reset.Invoke();
            if (String.IsNullOrEmpty(nextLevel))
            {
                Debug.WriteLine("Do we get here?");
                OnNoNextLevel(WinScreen.ID);
            }
            else
            {
                LoadLocal(nextLevel);
            }
            
        }
    }
}
