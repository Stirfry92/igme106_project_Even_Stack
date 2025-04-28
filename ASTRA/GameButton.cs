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
        Listener<bool> Pressed = new Listener<bool>(false);

        private Color DrawColor;


        public GameButton(Vector2 position,Vector2 size, string textureName) : base(position, ComponentOrigin.Center)
        {
            LocalContentManager lcm = LocalContentManager.Shared;
            this.Size = size;
            Image = lcm.GetTexture(textureName);

            DrawColor = Color.Red;
            Pressed.OnValueChanged += () =>
            {
                if (Pressed.Value)
                {
                    DrawColor = Color.Green;
                }
                else
                {
                    DrawColor = Color.Red;
                }
            };
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
            batch.Draw(Image, new Rectangle(TopLeftCorner.ToPoint(), Size.ToPoint()), DrawColor);
            
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
                Pressed.Value = true;
            }
        }
        internal override void Update(GameTime gameTime)
        {}

        internal override void Reset()
        {
            Pressed.Value = false;
        }
    }
}
