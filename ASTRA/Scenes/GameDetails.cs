using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA.Scenes
{
    internal static class GameDetails
    {
        
        /// <summary>
        /// Whether the game should be run in Testing Mode: A mode that only runs a sandbox scene for testing mechanics.
        /// </summary>
        internal const bool TestingMode = true;

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




    }
}
