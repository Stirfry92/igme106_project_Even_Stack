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
        private int LevelNum;
        private int Timer;
        private int x;
        private int y;
        private Player player;
        public Player Player { get { return player; } }

        public event Action<Rectangle> playerLocation;
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

        //level loading section
        /// <summary>
        /// loads a single level file
        /// </summary>
        public void LoadLevel(string file, GameObjectDelegate add, GameObjectDelegate delete)
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
                            level[j, i] = new GameButton(new Vector2(j * GameDetails.TileSize, i * GameDetails.TileSize), new Vector2(GameDetails.TileSize, GameDetails.TileSize), "Wall1");
                            break;
                        case 'D'://closed door
                            level[j, i] = new GameDoor(new Vector2(j * GameDetails.TileSize, i * GameDetails.TileSize), new Vector2(GameDetails.TileSize, GameDetails.TileSize), "Wall1");
                            break;
                        case 'd'://opened door

                            break;
                        case 'O'://obstacle
                            level[j, i] = new CollidableWall(new Vector2(j * GameDetails.TileSize, i * GameDetails.TileSize), new Vector2(GameDetails.TileSize, GameDetails.TileSize), "Wall1");
                            break;
                        case '-'://open space
                            // add floor class
                            break;
                        case 'E'://end

                            break;
                        case 'S'://start
                            player = new Player(new Vector2(j * GameDetails.TileSize, i * GameDetails.TileSize));
                            player.AddToParent = add;
                            player.RemoveFromParent = delete;

                            level[j, i] = player;
                            break;
                    }

                    add(level[j, i]);
                }
            }
            // sets up the level logic
            data = reader.ReadLine().Split('+');
            do
            {
                logic(data[0], data[1]);
                data = reader.ReadLine().Split('+');
            } while (data[0] != "//");
            reader.Close();
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
    }
}
