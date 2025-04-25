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
            Vector2 differenceInCenter = new Vector2(10, 0);

            StaticImage gameplayOverlay = new StaticImage("overlay", "blackoverlay", Vector2.Zero, ComponentOrigin.TopLeft);
            gameplayOverlay.Color = new Color(0xff, 0xff, 0xff, 0xc0);

            UI.AddComponent(gameplayOverlay);

            TextComponent info = new TextComponent("info", "Keep playing?", GameDetails.CenterOfScreen - new Vector2(0, 50), ComponentOrigin.BottomCenter);
            UI.AddComponent(info);


            Button cont = new Button("go_back", "Continue", GameDetails.CenterOfScreen + differenceInCenter, ComponentOrigin.TopLeft);
            cont.OnClick += () =>
            {
                SetScene(GameScreen.ID);
                GameScreen.ResetGame = GameScreen.IsGameOver;
            };

            UI.AddComponent(cont);

            Button mainmenu = new Button("menu", "Main Menu", GameDetails.CenterOfScreen - differenceInCenter, ComponentOrigin.TopRight);
            mainmenu.OnClick += () =>
            {
                SetScene(HomeScreen.ID);
                GameScreen.ResetGame = true;
            };

            UI.AddComponent(mainmenu);

            
        }

        internal override void Load()
        {

            //we already know this will be the game screen with this structure
            GameScreen = (GameScreen)GetScene(GameScreen.ID);

            bool successfulGrab = UI.TryGetComponent("info", out UIComponent component);

            if (GameScreen.IsGameOver && successfulGrab)
            {
                (component as TextComponent).SetText("Retry?");
            }
            else
            {
                (component as TextComponent).SetText("Continue?");
            }
        }

        internal override void Draw(SpriteBatch batch)
        {
            GameScreen.Draw(batch);


            base.Draw(batch);
        }
    }
}
