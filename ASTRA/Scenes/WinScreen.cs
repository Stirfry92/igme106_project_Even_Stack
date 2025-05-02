using ASTRA.UserInterface;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA.Scenes
{
    internal class WinScreen : Scene
    {
        internal new const string ID = "Win Screen";

        internal WinScreen() : base()
        {
            TextComponent text = new TextComponent("text", "You win!", "Standard", GameDetails.CenterOfScreen, ComponentOrigin.BottomCenter);
            UI.AddComponent(text);

            Button home = new Button("home", "Main Menu", GameDetails.CenterOfScreen + new Vector2(0, 10), ComponentOrigin.TopCenter);
            home.OnClick += () => SetScene(HomeScreen.ID);

            UI.AddComponent(home);
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
