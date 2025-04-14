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
        private float speed;                            //Represents the speed of the player

        private MouseState currentMState;               //Current state of the mouse (stored for input)
        private MouseState previousMState;              //Previous state of the mouse (stored for input)

        private KeyboardState currentKBState;
        private KeyboardState previousKBState;
        
        /// <summary>
        /// Creates a new player object at the current position.
        /// </summary>
        /// <param name="position"></param>
        public Player(Vector2 position) : base(position, ComponentOrigin.Center)
        {

            LocalContentManager lcm = LocalContentManager.Shared;

            //TODO: get a player asset. Comment this out if need be.
            /*Image = lcm.GetTexture("player");


            Size = new Vector2(Image.Width, Image.Height);
            */

            //TODO: Change this, this is temporary for testing purposes.
            this.Size = new Vector2(50, 50);
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

            //Three Phases.
            //Collect Input:

            //Execute movement:
            //Update the direction the player faces:
            dir = (currentMState.Position.ToVector2()) - Position;
            dir.Normalize(); 

            //Perform necessary "clean up" tasks.

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
