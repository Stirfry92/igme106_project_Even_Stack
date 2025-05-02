using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA
{
    internal class Floor : GameObject, IDrawable
    {
        /// <summary>
        /// Basic floor utilizing the gameobject inheritence. Allows for tile based placement.
        /// </summary>
        /// <param name="position"></param>
        public Floor(Vector2 position, Vector2 size, string textureName) : base(position, ComponentOrigin.Center)
        {
            LocalContentManager lcm = LocalContentManager.Shared;

            Image = lcm.GetTexture(textureName);
            this.Size = size;
        }

        /// <summary>
        /// The image of the floor.
        /// </summary>
        public Texture2D Image { get; }

        /// <summary>
        /// Updates the object. (Does nothing as a floor does nothing.)
        /// </summary>
        /// <param name="gameTime"></param>
        internal override void Update(GameTime gameTime) { }

        /// <summary>
        /// Draws out the asset
        /// </summary>
        /// <param name="batch"></param>
        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, new Rectangle(TopLeftCorner.ToPoint(), Size.ToPoint()), Color.White);
        }
    }
}
