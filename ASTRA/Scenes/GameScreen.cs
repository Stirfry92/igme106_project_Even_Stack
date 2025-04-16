using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA.Scenes
{
    internal class GameScreen : Scene
    {
        internal new const string ID = "Game Screen";

        internal GameScreen() : base()
        {
            this.Add(new Player(new Vector2(500, 500)));
        }
    }

    
}
