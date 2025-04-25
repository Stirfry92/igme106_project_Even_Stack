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
        private int LevelNum;
        private int Timer;
        private int x;
        private int y;
        private Player player;
        /// <summary>
        /// draws level from level array
        /// </summary>
        public void Update( GameTime gt) 
        {
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
                    if (level[j, i] != null && level[j,i] is IDrawable drawable)
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

            for (int i = 0; i < y; i++)
            {
                asd = reader.ReadLine().ToCharArray();
                for (int j = 0; j < x; j++)
                {
                    switch (asd[j])
                    {
                        case 'X'://wall
                            level[j, i] = new CollidableWall(new Vector2(j * GameDetails.TileSize, i * GameDetails.TileSize),new Vector2(GameDetails.TileSize, GameDetails.TileSize));
                            break;
                        case '!'://hole
                            level[j, i] = new CollidableWall(new Vector2(j * GameDetails.TileSize, i * GameDetails.TileSize), new Vector2(GameDetails.TileSize, GameDetails.TileSize));

                            // needs hole class
                            break;
                        case 'b'://button

                            break;
                        case 'd'://closed door

                            break;
                        case 'D'://opened door

                            break;
                        case 'O'://obstacle
                            level[j, i] = new CollidableWall(new Vector2(j * GameDetails.TileSize, i * GameDetails.TileSize), new Vector2(GameDetails.TileSize, GameDetails.TileSize));
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
            reader.Close();
        }

        /// <summary>
        /// connects a button with doors (not done needs buttons)
        /// </summary>
        public void logic()
        {
            //your mom
            //lock IN
        }
    }
}
