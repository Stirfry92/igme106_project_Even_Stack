using ASTRA.UserInterface;
using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace ASTRA.Scenes
{
    internal class MechanicsContextScreen : Scene
    {
        internal new const string ID = "Mechanics Context Screen";

        internal KeyboardState PreviousKeyboardState = new KeyboardState();

        internal bool Visited = false;

        internal MechanicsContextScreen() : base()
        {
            TextComponent explanation = new TextComponent(
                "explanation",
                "Hold SPACE to charge a jump.\nAim using the mouse to direct your character!\nPress LEFT CLICK to fire hammers to alter your trajectory mid-air!\n\nPress space to continue...",
                "Standard", GameDetails.CenterOfScreen, ComponentOrigin.BottomCenter);

            UI.AddComponent(explanation);
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            KeyboardState CurrentState = Keyboard.GetState();

            if (CurrentState.IsKeyUp(Keys.Space) && PreviousKeyboardState.IsKeyDown(Keys.Space))
            {
                SetScene(GameScreen.ID);
                Visited = true;
            }

            PreviousKeyboardState = CurrentState;
            
        }

        internal override void Load()
        {
            if (Visited)
            {
                SetScene(GameScreen.ID);
            }
        }

    }
}
