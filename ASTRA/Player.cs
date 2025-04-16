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

    /// <summary>
    /// Represents the player object.
    /// 
    /// NOTES:
    /// Currently working on maping out the movement in a way that I understand
    /// in combination with understanding the new code.
    /// 
    /// Most of the fields have no functionality but are there simply because I believe I will use them.
    /// Until the functionality of all the interfaces is complete, the player is non-functional.
    /// 
    /// This class is currently FAR FROM COMPLETE.
    /// YOU HAVE BEEN WARNED!
    /// </summary>
    internal class Player : GameObject, ICollidable, IDrawable
    {
        //Fields:
        private Vector2 dir;                            //Represents the direction the player "faces".
        private float speed;                            //Represents the speed added to the direction when the player "pushes"
        private Vector2 velocity;                       //Represents the velocity of the player

        private MouseState currentMState;               //Current state of the mouse (stored for input)
        private MouseState previousMState;              //Previous state of the mouse (stored for input)

        private KeyboardState currentKBState;           //Current state of the keyboard (stored for input)
        private KeyboardState previousKBState;          //Previous state of the keyboard (stored for input)
        
        /// <summary>
        /// Creates a new player object at the current position.
        /// </summary>
        /// <param name="position"></param>
        public Player(Vector2 position) : base(position, ComponentOrigin.Center)
        {

            LocalContentManager lcm = LocalContentManager.Shared;

            //TODO: get a player asset. Comment this out if need be.
            Image = lcm.GetTexture("blank");


            //Size = new Vector2(Image.Width, Image.Height);
            
            
            //TODO: Change this, this is temporary for testing purposes.
            this.Size = new Vector2(50, 50);
            this.speed = 5f;
            this.velocity = new Vector2(0, 0);
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
            //Use an intersection rectangle to determine logic, in combination with the bounds of collision.
            Rectangle intersection = Rectangle.Intersect(CollisionBounds, other.CollisionBounds);

            //In each of the below instances, if the player is holding down space they will bounce, otherwise they will grab onto the wall and lose their velocity.
            //Instance where the player is to the left of the collidable surface
            if (intersection.Height > intersection.Width && CollisionBounds.X <= other.CollisionBounds.X)
            {
                Position = new Vector2(Position.X - intersection.Width, Position.Y);
                if (currentKBState.IsKeyDown(Keys.Space))
                {
                    velocity = new Vector2(-0.5f * velocity.X, velocity.Y);
                }
                else
                {
                    velocity = Vector2.Zero;
                }
                
            }
            //Instance where the player is to the right of the collidable surface
            else if (intersection.Height > intersection.Width && CollisionBounds.X > other.CollisionBounds.X)
            {
                Position = new Vector2(Position.X + intersection.Width, Position.Y);
                if (currentKBState.IsKeyDown(Keys.Space))
                {
                    velocity = new Vector2(-0.5f * velocity.X, velocity.Y);
                }
                else
                {
                    velocity = Vector2.Zero;
                }
            }
            //Instance where the player is above the collidable surface
            if (intersection.Height <= intersection.Width && CollisionBounds.Y <= other.CollisionBounds.Y)
            {
                Position = new Vector2(Position.X, Position.Y - intersection.Height);
                if (currentKBState.IsKeyDown(Keys.Space))
                {
                    velocity = new Vector2(velocity.X, -0.5f * velocity.Y);
                }
                else
                {
                    velocity = Vector2.Zero;
                }
            }
            //Instance where the player is below the collidable surface
            else if (intersection.Height <= intersection.Width && CollisionBounds.Y > other.CollisionBounds.Y)
            {
                Position = new Vector2(Position.X , Position.Y + intersection.Height);
                if (currentKBState.IsKeyDown(Keys.Space))
                {
                    velocity = new Vector2(velocity.X, -0.5f * velocity.Y);
                }
                else
                {
                    velocity = Vector2.Zero;
                }
            }
        }

        /// <summary>
        /// Updates the player object.
        /// </summary>
        /// <param name="gameTime"></param>
        internal override void Update(GameTime gameTime)
        {
            //TODO: add update logic for the movement system here.

            //Three Phases.
            //Collect Input:
            //Set current states
            currentKBState = Keyboard.GetState();
            currentMState = Mouse.GetState();
            
            //Execute movement:
            //Update the direction the player faces:
            if (currentKBState.IsKeyUp(Keys.Space) && previousKBState.IsKeyDown(Keys.Space))
            {
                dir = (currentMState.Position.ToVector2()) - Position;
                dir.Normalize();

                velocity = velocity + dir * speed;
            }
            
            Position = Position + velocity;

            

            //Perform necessary "clean up" tasks:
            //Set Previous states
            previousKBState = currentKBState;
            previousMState = currentMState;
        }

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
