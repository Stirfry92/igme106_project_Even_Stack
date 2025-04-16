using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace ASTRA.UserInterface
{

    /// <summary>
    /// Represents a button that the player can interact with.
    /// </summary>
    internal class Button : UIComponent, IDrawable
    {
        /// <summary>
        /// The background of the button texture.
        /// </summary>
        public new Texture2D Image { get; }

        /// <summary>
        /// The text that will be displayed on the image.
        /// </summary>
        internal string Text { get; private set; }

        /// <summary>
        /// The render point by which the text is rendered from.
        /// </summary>
        private Vector2 TextRenderPoint;

        /// <summary>
        /// The font that the text is rendered from.
        /// </summary>
        private SpriteFont Font;





        private enum ButtonState
        {
            Nothing,
            Hovered,
            Pressed
        }

        /// <summary>
        /// The current state of the button.
        /// </summary>
        private ButtonState State;
        

        /// <summary>
        /// The events that should occur when the button is hovered (mouse enters the button bounds).
        /// </summary>
        internal event UpdateDelegate OnHover;

        /// <summary>
        /// The events that should occur when the button is clicked (mouse pressed then released).
        /// </summary>
        internal event UpdateDelegate OnClick;




        /// <summary>
        /// Creates a button
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="position"></param>
        /// <param name="origin"></param>
        public Button(string ID, string text, Vector2 position, ComponentOrigin origin) : base(ID, position, origin)
        {

            LocalContentManager lcm = LocalContentManager.Shared;

            //get the background image (COMMENT IF NOT WORKING)
            Image = lcm.GetTexture("button");

            //get the font for the button (COMMENT IF NOT WORKING)
            //Font = lcm.GetFont("blah");

            //SetText(text);

            Size = Image.Bounds.Size.ToVector2();
        }

        internal void SetText(string text)
        {
            Text = text;
            TextRenderPoint = TopLeftCorner + Size * 0.5f - Font.MeasureString(Text) * 0.5f;
        }



        /// <summary>
        /// Draws the button.
        /// </summary>
        /// <param name="batch"></param>
        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, TopLeftCorner, Color.White);
            //batch.DrawString(Font, Text, TextRenderPoint, Color.White);
        }


        internal override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            Rectangle buttonBounds = new Rectangle(TopLeftCorner.ToPoint(), Size.ToPoint());
            
            //FSM
            switch (State)
            {

                //nothing: can go to hover when mouse goes over element
                case ButtonState.Nothing:
                    {
                        
                        if (buttonBounds.Contains(mouseState.Position))
                        {
                            State = ButtonState.Hovered;
                            OnHover?.Invoke();
                            break;
                        }

                        break;
                    }

                //hovered: can go to nothing when mouse leaves element bounds
                //          can go to pressed when left mouse button is clicked.
                case ButtonState.Hovered:
                    {

                        if (!buttonBounds.Contains(mouseState.Position))
                        {
                            State = ButtonState.Nothing;
                            break;
                        }

                        if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                        {
                            State = ButtonState.Pressed;
                            break;
                        }


                        break;
                    }

                //pressed: can go to hover when mouse released left mouse button
                //          can go to nothing if mouse leaves element bounds
                case ButtonState.Pressed:
                    {
                        if (!buttonBounds.Contains(mouseState.Position))
                        {
                            State = ButtonState.Nothing;
                            break;
                        }

                        if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                        {
                            State = ButtonState.Hovered;
                            OnClick?.Invoke();
                            OnHover?.Invoke();
                            
                        }

                        break;
                    }
            }
        }
    }
}
