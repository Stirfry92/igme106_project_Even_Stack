using ASTRA.UserInterface;
using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace ASTRA.Scenes
{
    internal class PauseScreen : Scene
    {

        /// <summary>
        /// The string ID of the pause screen.
        /// </summary>
        internal new const string ID = "Pause Screen";

        /// <summary>
        /// A reference to the game screen.
        /// </summary>
        private GameScreen GameScreen;


        internal PauseScreen() : base()
        {
            Button cont = new Button("go_back", "Continue", new Vector2(5, 5), ComponentOrigin.TopLeft);
            cont.OnClick += () => SetScene(GameScreen.ID);

            UI.AddComponent(cont);

            Button mainmenu = new Button("menu", "Main Menu", new Vector2(200, 5), ComponentOrigin.TopLeft);
            mainmenu.OnClick += () => SetScene(HomeScreen.ID);

            UI.AddComponent(mainmenu);
        }

        internal override void Load()
        {

            //we already know this will be the game screen with this structure
            GameScreen = (GameScreen)GetScene(GameScreen.ID);
        }

        internal override void Draw(SpriteBatch batch)
        {
            GameScreen.Draw(batch);


            base.Draw(batch);
        }
    }
}
