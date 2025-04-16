using ASTRA.UserInterface;
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

        public void DrawLevel(SpriteBatch asd)
        {
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    if (level[j, i] != null)
                    {
                        level[j, i].Draw(asd);
                    }

                }
            }
        }

        //level loading section
        /// <summary>
        /// loads a single level file
        /// </summary>
        public void LoadLevel(string file)
        {
            Player tempWall;
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
                            tempWall = new Player(new Microsoft.Xna.Framework.Vector2(j * 50, i * 50));
                            level[j, i] = tempWall;
                            //new Player(new Microsoft.Xna.Framework.Vector2(x*50,y*50));
                            break;
                        case '!'://hole
                            level[j, i] = new Player(new Microsoft.Xna.Framework.Vector2(j * 50, i * 50));
                            break;
                        case 'b'://button
                            level[j, i] = new Button("ass","dasdas", new Microsoft.Xna.Framework.Vector2(j * 50, i * 50), new ComponentOrigin());
                            break;
                        case 'd'://closed door

                            break;
                        case 'D'://opened door

                            break;
                        case 'O'://obstacle
                            tempWall = new Player(new Microsoft.Xna.Framework.Vector2(j * 50, i * 50));
                            level[j, i] = tempWall;
                            break;
                        case '-'://open space
                            
                            break;
                        case 'E'://end

                            break;
                        case 'S'://start

                            break;
                    }
                }
            }
            reader.Close();
        }

        /// <summary>
        /// connects a button with doors
        /// </summary>
        public void logic()
        {
            //your mom
        }
    }
}
