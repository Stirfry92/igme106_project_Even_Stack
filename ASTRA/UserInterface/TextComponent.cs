using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA.UserInterface
{
    internal class TextComponent : UIComponent
    {

        private readonly SpriteFont Font;

        internal string Text
        {
            get
            {
                return text;
            }
        }

        private string text;

        internal Color TextColor { get; set; } = Color.White;




        public TextComponent(string ID, string text, string font, Vector2 position, ComponentOrigin origin) : base(ID, position, origin)
        {

            LocalContentManager lcm = LocalContentManager.Shared;

            Font = lcm.GetFont(font);
            SetText(text);
        }

        internal void SetText(string text)
        {
            this.text = text;
            Size = Font.MeasureString(this.text);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.DrawString(Font, Text, TopLeftCorner, TextColor);
        }

        internal override void Update(GameTime gameTime)
        {
            
        }
    }
}
