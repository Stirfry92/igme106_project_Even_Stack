using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA
{
    class GameButton : GameObject, IDrawable, ICollidable
    {
        /// <summary>
        /// tells weather or not to press button
        /// </summary>
        bool DrawButton = true;
        public GameButton(Vector2 position,Vector2 size, string textureName) : base(position, ComponentOrigin.Center)
        {
            LocalContentManager lcm = LocalContentManager.Shared;
            this.Size = size;
            Image = lcm.GetTexture(textureName);
        }

        /// <summary>
        /// tells doors that button is pressed by the player
        /// </summary>
        public event EventHandler IsPressed;

        public Texture2D Image { get; }

        public void Collide(ICollidable other)
        {
        }

        public void Draw(SpriteBatch batch)
        {
            if (DrawButton)
            {
                batch.Draw(Image, new Rectangle(TopLeftCorner.ToPoint(), Size.ToPoint()), Color.Red);
            }
        }
        public Rectangle CollisionBounds
        {
            get
            {
                return new Rectangle(TopLeftCorner.ToPoint(), Size.ToPoint());
            }
        }
        public bool CollidesWith(ICollidable other)
        {
            return CollisionBounds.Intersects(other.CollisionBounds);
        }

        internal void CollidesWithPlayer(Rectangle playerLocation)
        {
            // if player/object overlaps with button it calls event
            Rectangle ass = playerLocation;
            if (CollisionBounds.Intersects(playerLocation))
            {
                IsPressed.Invoke(this, new EventArgs());
                DrawButton = false;
            }
        }
        internal override void Update(GameTime gameTime)
        {}
    }
}
