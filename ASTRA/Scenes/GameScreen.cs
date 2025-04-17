using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace ASTRA.Scenes
{
    internal class GameScreen : Scene
    {

        /// <summary>
        /// The ID for the game sequence
        /// </summary>
        internal new const string ID = "Game Screen";

        /// <summary>
        ///The previous keyboard state.
        /// </summary>
        private KeyboardState PreviousKeyboardState;

        internal GameScreen() : base()
        {
            this.Add(new Player(new Vector2(500, 500)));
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            KeyboardState kbstate = Keyboard.GetState();


            //single press of the escape keyboard
            if (PreviousKeyboardState.IsKeyDown(Keys.Escape) && kbstate.IsKeyUp(Keys.Escape))
            {
                SetScene(PauseScreen.ID);
            }




            PreviousKeyboardState = kbstate;
            Clean();
        }
    }

    
}
