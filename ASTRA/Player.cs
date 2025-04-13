using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA
{

    /// <summary>
    /// Represents the player object.
    /// </summary>
    internal class Player : GameObject, ICollidable, IDrawable
    {
        
        
        
        /// <summary>
        /// Creates a new player object at the current position.
        /// </summary>
        /// <param name="position"></param>
        public Player(Vector2 position) : base(position, ComponentOrigin.Center)
        {

            LocalContentManager lcm = LocalContentManager.Shared;

            //TODO: get a player asset. Comment this out if need be.
            Image = lcm.GetTexture("player");


            Size = new Vector2(Image.Width, Image.Height);
        }

        /// <summary>
        /// The player's collision bounds. In this instance, it takes up the whole width and height of the element.
        /// </summary>
        public Rectangle CollisionBounds
        {
            get
            {
                return new Rectangle(Position.ToPoint(), Size.ToPoint());
            }
        }


        /// <summary>
        /// The image of the player.
        /// </summary>
        public Texture2D Image { get; }


        /// <summary>
        /// Handles all collisions (without updating the other object's status!).
        /// </summary>
        /// <param name="other"></param>
        public void Collide(ICollidable other)
        {
            //TODO: add in base logic for collision (like walls).
        }

        /// <summary>
        /// Updates the player object.
        /// </summary>
        /// <param name="gameTime"></param>
        internal override void Update(GameTime gameTime)
        {
            //TODO: add update logic for the movement system here.
        }

        /// <summary>
        /// Draws out the asset
        /// </summary>
        /// <param name="batch"></param>
        void IDrawable.Draw(SpriteBatch batch)
        {
            batch.Draw(Image, TopLeftCorner, Color.White);
        }
    }
}
