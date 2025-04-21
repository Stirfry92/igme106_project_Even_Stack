using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA.UserInterface
{
    internal class StaticImage : UIComponent
    {

        /// <summary>
        /// The render color to use for this image.
        /// </summary>
        internal Color Color { get; set; } = Color.White;

        /// <summary>
        /// Creates a new static image to be placed on the UI.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="spriteName"></param>
        /// <param name="position"></param>
        /// <param name="origin"></param>
        public StaticImage(string ID, string spriteName, Vector2 position, ComponentOrigin origin) : base(ID, position, origin)
        {
            Image = LocalContentManager.Shared.GetTexture(spriteName);

            Size = Image.Bounds.Size.ToVector2();
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, TopLeftCorner, Color);
        }

        internal override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {}
    }
}
