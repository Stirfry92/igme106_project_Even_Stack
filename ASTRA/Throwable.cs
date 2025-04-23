using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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
        /// Creates a new throwable object at the current position.
        /// </summary>
        /// <param name="position"></param>
        public Throwable(Vector2 position) : base(position, ComponentOrigin.Center)
        {

            //get the local content manager instance
            LocalContentManager lcm = LocalContentManager.Shared;

            Image = lcm.GetTexture("hammer");

            this.Size = new Vector2(64, 64);
            this.velocity = new Vector2(0, 0);
        }

        public Throwable(Vector2 position, Vector2 velocity) : base(position, ComponentOrigin.Center)
        {
            //get the local content manager instance
            LocalContentManager lcm = LocalContentManager.Shared;

            Image = lcm.GetTexture("hammer");

            this.Size = new Vector2(64, 64);
            this.velocity = velocity;
            //this.canPickup = true;
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
            if (other is CollidableWall wall)
            {
                Collide(wall);
            }

            if (other is Player && !JustThrown)
            {
                Remove(this);
            }
            
            
        }
        private void Collide(CollidableWall other)
        {
            //TODO: add in base logic for collision (like walls).
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

            /*
            //Now that the object has struck a wall, we can pick it up again on the rebound:
            canPickup = true;
            */
        }

        /// <summary>
        /// Updates the throwable object.
        /// </summary>
        /// <param name="gameTime"></param>
        internal override void Update(GameTime gameTime)
        {
            //TODO: add update logic for the movement system here.

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


