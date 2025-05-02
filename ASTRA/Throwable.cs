using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ASTRA.Scenes;

namespace ASTRA
{
    internal class Throwable : GameObject, ICollidable, IDrawable
    {
        //Fields:
        private Vector2 velocity;                       //Represents the velocity of the object
        private bool canPickup;                         //Represents if the throwable is ready to be picked up.

        /// <summary>
        /// The logic passed from the parent scene so that a throwable has the ability to remove itself from the parent.
        /// </summary>
        internal GameObjectDelegate Remove;

        /// <summary>
        /// Whether the throwable was just thrown.
        /// </summary>
        internal bool JustThrown = true;

        /// <summary>
        /// The amount that the hammer is rotated (in radians).
        /// </summary>
        private float Rotation;

        /// <summary>
        /// Creates a new throwable object at the current position.
        /// </summary>
        /// <param name="position"></param>
        public Throwable(Vector2 position) : base(position, ComponentOrigin.Center)
        {
            //Get the local content manager instance
            LocalContentManager lcm = LocalContentManager.Shared;

            Image = lcm.GetTexture("hammer");

            this.Size = new Vector2(64, 64);
            this.velocity = new Vector2(0, 0);

            Rotation = (float)(Random.Shared.NextDouble() * Math.PI * 2);
        }

        public Throwable(Vector2 position, Vector2 velocity) : base(position, ComponentOrigin.Center)
        {
            //Get the local content manager instance
            LocalContentManager lcm = LocalContentManager.Shared;

            Image = lcm.GetTexture("hammer");

            this.Size = new Vector2(GameDetails.TileSize * 0.8f, GameDetails.TileSize * 0.8f);
            this.velocity = velocity;
        }

        /// <summary>
        /// The player's collision bounds. In this instance, it takes up the whole width and height of the element.
        /// </summary>
        public Rectangle CollisionBounds
        {
            get
            {
                return new Rectangle(TopLeftCorner.ToPoint(), Size.ToPoint());
            }
        }

        /// <summary>
        /// The image of the object.
        /// </summary>
        public Texture2D Image { get; }


        /// <summary>
        /// Handles all collisions (without updating the other object's status!).
        /// </summary>
        /// <param name="other"></param>
        public void Collide(ICollidable other)
        {
            if (other is CollidableWall || other is GameDoor door)
            {
                CollideBounce(other);
            }

            if (other is Player && !JustThrown)
            {
                Remove(this);
            }
        }
        private void CollideBounce(ICollidable other)
        {
            //Use an intersection rectangle to determine logic, in combination with the bounds of collision.
            Rectangle intersection = Rectangle.Intersect(CollisionBounds, other.CollisionBounds);

            if (intersection.Height > intersection.Width && CollisionBounds.X <= other.CollisionBounds.X)
            {
                Position = new Vector2(Position.X - intersection.Width, Position.Y);
                velocity = new Vector2(-velocity.X, velocity.Y);

            }
            else if (intersection.Height > intersection.Width && CollisionBounds.X > other.CollisionBounds.X)
            {
                Position = new Vector2(Position.X + intersection.Width, Position.Y);
                velocity = new Vector2(-velocity.X, velocity.Y);
            }
            else if (intersection.Height <= intersection.Width && CollisionBounds.Y <= other.CollisionBounds.Y)
            {
                Position = new Vector2(Position.X, Position.Y - intersection.Height);
                velocity = new Vector2(velocity.X, -velocity.Y);
            }
            else if (intersection.Height <= intersection.Width && CollisionBounds.Y > other.CollisionBounds.Y)
            {
                Position = new Vector2(Position.X, Position.Y + intersection.Height);
                velocity = new Vector2(velocity.X, -velocity.Y);
            }
        }

        /// <summary>
        /// Updates the throwable object.
        /// </summary>
        /// <param name="gameTime"></param>
        internal override void Update(GameTime gameTime)
        {
            //Apply whatever motion to the object here:
            Position = Position + velocity;
        }

        /// <summary>
        /// Draws out the asset.
        /// </summary>
        /// <param name="batch"></param>
        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, CollisionBounds, Color.White);
        }

        /// <summary>
        /// Whether the throwable collides with another collidable.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool CollidesWith(ICollidable other)
        {
            return CollisionBounds.Intersects(other.CollisionBounds);
        }
    }
}


