using System;
using System.Collections.Generic;
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


        //level loading section
        /// <summary>
        /// loades single level file
        /// </summary>
        public void LoadLevel(string file)
        {
            Char[] asd;
            reader = new StreamReader(file);
            string[] data = reader.ReadLine().Split(',');
            name = data[0];
            LevelNum = int.Parse(data[1]);
            Timer = int.Parse(data[2]);
            x = int.Parse(data[3]);
            y = int.Parse(data[4]);
            for (int i = 5; i < y; i++) 
            {
                asd = reader.ReadLine().ToCharArray();
                for (int j = 0; j < x; j++) 
                {
                    switch (asd[x]) 
                    {
                        case 'X'://wall
                            break;
                        case '!'://hole
                            break;
                        case 'b'://button
                            break;
                        case 'd'://closed door
                            break;
                        case 'D'://oped door
                            break;
                        case 'O'://obstacle
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
