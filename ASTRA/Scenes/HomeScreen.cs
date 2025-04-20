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
            Vector2 differenceInCenter = new Vector2(10, 0);


            Button start = new Button("start", "Start Game", GameDetails.CenterOfScreen - differenceInCenter, ComponentOrigin.TopRight);
            start.OnClick += () => { SetScene(GameScreen.ID); };
            UI.AddComponent(start);

            Button exit = new Button("exit", "Exit", GameDetails.CenterOfScreen + differenceInCenter, ComponentOrigin.TopLeft);
            exit.OnClick += () => { ExitGame(); };
            UI.AddComponent(exit);

            StaticImage logo = new StaticImage("logo", "astralogo", GameDetails.CenterOfScreen, ComponentOrigin.BottomCenter);
            UI.AddComponent(logo);
        }

        

        

    }
}
