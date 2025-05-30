﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA
{
    internal static class GameDetails
    {
        
        /// <summary>
        /// Whether the game should be run in Testing Mode: A mode that only runs a sandbox scene for testing mechanics.
        /// </summary>
        internal const bool TestingMode = false;

        /// <summary>
        /// Whether the game should be run with the player in God Mode!
        /// </summary>
        internal const bool GodMode = false;


        /// <summary>
        /// The size of each tile.
        /// </summary>
        internal const int TileSize = 50;

        /// <summary>
        /// The relative game width.
        /// </summary>
        internal const int GameWidth = 1920;

        /// <summary>
        /// The relative game height
        /// </summary>
        internal const int GameHeight = 1080;

        /// <summary>
        /// The center of the screen.
        /// </summary>
        internal static Vector2 CenterOfScreen
        {
            get
            {
                return new Vector2(GameWidth*0.5f, GameHeight*0.5f);
            }
        }


        internal readonly static Rectangle GameBounds = new Rectangle(0, 0, GameWidth, GameHeight);




    }
}
