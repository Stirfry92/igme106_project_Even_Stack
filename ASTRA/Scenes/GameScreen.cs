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
            this.Add(new Player(new Vector2(800, 500)));

            for (int i = 600; i <= 1600; i += 50)
            {
                this.Add(new CollidableWall(new Vector2(i, 200), new Vector2(50, 50)));
                
                this.Add(new CollidableWall(new Vector2(i, 800), new Vector2(50, 50)));
                
            }
            for (int i = 200; i < 800; i += 50)
            {
                this.Add(new CollidableWall(new Vector2(1600, i), new Vector2(50, 50)));
                this.Add(new CollidableWall(new Vector2(600, i), new Vector2(50, 50)));
            }
            
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

        /// <summary>
        /// Resets the game
        /// TODO: Eventually add in the game logic to reset the player.
        /// </summary>
        internal override void Reset()
        {
            base.Reset();
        }
    }

    
}
