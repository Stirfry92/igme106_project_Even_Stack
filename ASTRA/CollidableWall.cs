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
    class CollidableWall : GameObject, IDrawable, ICollidable
    {



        /// <summary>
        /// TODO: This class is temporary for playtesting. We should remove this an place in a more stable structure
        /// </summary>
        /// <param name="position"></param>
        public CollidableWall(Vector2 position, Vector2 size) : base(position, ComponentOrigin.Center)
        {

            LocalContentManager lcm = LocalContentManager.Shared;

            //TODO: get a default asset. Comment this out if need be.
            Image = lcm.GetTexture("tile");


            //Size = new Vector2(Image.Width, Image.Height);
            this.Size = size;
        }

        /// <summary>
        /// The collision bounds. In this instance, it takes up the whole width and height of the element.
        /// </summary>
        public Rectangle CollisionBounds
        {
            get
            {
                return new Rectangle(TopLeftCorner.ToPoint(), Size.ToPoint());
            }
        }



        /// <summary>
        /// The image of the player.
        /// </summary>
        public Texture2D Image { get; }


        /// <summary>
        /// Handles all collisions (without updating the other object's status!).
        /// Should be called by a manager class which checks the collision between the player and all other objects.
        /// This way, the player knows how to react when it collides with something, but does not handle checking collision.
        /// </summary>
        /// <param name="other"></param>
        public void Collide(ICollidable other)
        {
        }

        /// <summary>
        /// Updates the object. (Does nothing as a wall just kind of... sits there.)
        /// </summary>
        /// <param name="gameTime"></param>
        internal override void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Draws out the asset
        /// </summary>
        /// <param name="batch"></param>
        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, new Rectangle(TopLeftCorner.ToPoint(), Size.ToPoint()), Color.White);
        }

        public bool CollidesWith(ICollidable other)
        {
            return CollisionBounds.Intersects(other.CollisionBounds);
        }
    }
}
