using ASTRA.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ASTRA.Scenes
{
    internal class HomeScreen : Scene
    {
        internal new const string ID = "Home Screen";

        internal HomeScreen() : base()
        {
            Button start = new Button("start", "Start Game", new Vector2(0, 5), ComponentOrigin.TopLeft);

            UI.AddComponent(start);

        }

    }
}
