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
    /// 
    ///
    public enum PlayerState
    {
        Grounded,
        Floating
    }

    internal class Player : GameObject, ICollidable, IDrawable
    {
        //Fields:
        private Vector2 dir;                            //Represents the direction the player "faces".
        private const float MaximumPushSpeed = 12f;     //Represents the maximum speed attainable by pushing.
        private const float MinimumPushSpeed = 5f;      //Represents the minimum speed attainable by pushing.
        private float speed;                            //Represents the speed added to the direction when the player "pushes"
        private float pushChargeTime;                   //Represents the amount of time charging the push.
        private Vector2 velocity;                       //Represents the velocity of the player

        private MouseState currentMState;               //Current state of the mouse (stored for input)
        private MouseState previousMState;              //Previous state of the mouse (stored for input)

        private KeyboardState currentKBState;           //Current state of the keyboard (stored for input)
        private KeyboardState previousKBState;          //Previous state of the keyboard (stored for input)

        private const float TotalTimeToReact = 0.25f;   //Maximum time the player has to react to a collision (seconds)
        private float timeToReact;                      //Time left for the player to react to a collision.

        private PlayerState state;                      //Current player state.

        /// <summary>
        /// The direction in which the player will move.
        /// </summary>
        private Vector2 DirectionVector;

        /// <summary>
        /// The arrow texture that will be used for indicating player movement.
        /// </summary>
        private Texture2D DirectionVectorImage;

        
        
        /// <summary>
        /// Creates a new player object at the current position.
        /// </summary>
        /// <param name="position"></param>
        public Player(Vector2 position) : base(position, ComponentOrigin.Center)
        {

            //get the local content manager instance
            LocalContentManager lcm = LocalContentManager.Shared;

            //TODO: get a player asset. Comment this out if need be.
            Image = lcm.GetTexture("editedAstronaut");

            //get the direction vector
            DirectionVectorImage = lcm.GetTexture("directionTriangle");


            //Size = new Vector2(Image.Width, Image.Height);


            //TODO: Change this, this is temporary for testing purposes.
            this.Size = new Vector2(Image.Width, Image.Height);
            //this.Size = new Vector2(50, 50);
            this.speed = 5f;
            this.velocity = new Vector2(0, 0);
            this.timeToReact = TotalTimeToReact;
            this.state = PlayerState.Grounded;
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
        /// The current player state.
        /// </summary>
        public PlayerState State
        {
            get { return state; }
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
                    velocity = new Vector2(-velocity.X, velocity.Y);
                }
                else
                {
                    velocity = Vector2.Zero;
                    state = PlayerState.Grounded;
                }
                
            }
            //Instance where the player is to the right of the collidable surface
            else if (intersection.Height > intersection.Width && CollisionBounds.X > other.CollisionBounds.X)
            {
                Position = new Vector2(Position.X + intersection.Width, Position.Y);
                if (currentKBState.IsKeyDown(Keys.Space))
                {
                    velocity = new Vector2(-velocity.X, velocity.Y);
                }
                else
                {
                    velocity = Vector2.Zero;
                    state = PlayerState.Grounded;
                }
            }
            //Instance where the player is above the collidable surface
            if (intersection.Height <= intersection.Width && CollisionBounds.Y <= other.CollisionBounds.Y)
            {
                Position = new Vector2(Position.X, Position.Y - intersection.Height);
                if (currentKBState.IsKeyDown(Keys.Space))
                {
                    velocity = new Vector2(velocity.X, -velocity.Y);
                }
                else
                {
                    velocity = Vector2.Zero;
                    state = PlayerState.Grounded;
                }
            }
            //Instance where the player is below the collidable surface
            else if (intersection.Height <= intersection.Width && CollisionBounds.Y > other.CollisionBounds.Y)
            {
                Position = new Vector2(Position.X , Position.Y + intersection.Height);
                if (currentKBState.IsKeyDown(Keys.Space))
                {
                    velocity = new Vector2(velocity.X, -velocity.Y);
                }
                else
                {
                    velocity = Vector2.Zero;
                    state = PlayerState.Grounded;
                }
            }

            //Since the player collided, give them time to react to the collision (push off from the wall).
            //TODO: Decide on whether we want the cyotye time. This currentl gives a major buff for when the player needs to react.
            //state = PlayerState.Grounded; 
            timeToReact = TotalTimeToReact;
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

            //update the direction vector each frame
            DirectionVector = currentMState.Position.ToVector2() - Position;
            DirectionVector.Normalize();

            
            //Execute movement:
            //Update the direction the player faces:
            switch(state)
            {
                case PlayerState.Grounded:
                    //First, reduce the amount of time the player has to react to a collision:
                    timeToReact -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    //Then, determine the keyboard state of the user and decide what to do from there:
                    //Player releases spacebar:
                    if (currentKBState.IsKeyUp(Keys.Space) && previousKBState.IsKeyDown(Keys.Space))
                    {
                        //Set a direction to where the mouse is pointing.
                        dir = (currentMState.Position.ToVector2()) - Position;
                        dir.Normalize();

                        //Determine speed and velocity based on how long the spacebar was held. 
                        speed = MathHelper.Clamp(MaximumPushSpeed * pushChargeTime, MinimumPushSpeed, MaximumPushSpeed);
                        velocity = velocity + dir * speed;

                        //Only switch to a different state if the time to react is over, and the player has pushed.
                        if (timeToReact < 0)
                        {
                            state = PlayerState.Floating;
                            timeToReact = TotalTimeToReact;
                        }  
                    }
                    //Player holds spacebar for maximum time alaughted:
                    else if (pushChargeTime > 1 )
                    {
                        //Reset relevant fields.
                        pushChargeTime = 0;
                        speed = 0;

                        //Set a direction to where the mouse is pointing.
                        dir = (currentMState.Position.ToVector2()) - Position;
                        dir.Normalize();

                        //Determine velocity based on the Max speed.
                        velocity = velocity + dir * MaximumPushSpeed;

                        //Only switch to a different state if the time to react is over, and the player has pushed.
                        if (timeToReact < 0)
                        {
                            state = PlayerState.Floating;
                            timeToReact = TotalTimeToReact;
                        }
                    }
                    //The player holds down spacebar:
                    else if (currentKBState.IsKeyDown(Keys.Space) && previousKBState.IsKeyDown(Keys.Space))
                    {
                        //Increase the field tracking the amount of time the player has held the spacebar.
                        pushChargeTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    //If the spacebar isn't pressed.
                    else if (currentKBState.IsKeyUp(Keys.Space))
                    {
                        //Set the charge timer to zero, and the speed also to zero (No charge is being stored)
                        pushChargeTime = 0;
                        speed = 0;
                    }
                    break;

                case PlayerState.Floating:

                    //this is inherently handled in the collision marker since anytime there are collisions, the player will be floating.
                    break;
            }

            //Once the input handling is done, apply whatever motion to the player's position here:
            Position = Position + velocity;

            //Perform necessary "clean up" tasks:
            //Set Previous states
            previousKBState = currentKBState;
            previousMState = currentMState;
            
        }

        /// <summary>
        /// Draws out the asset.
        /// </summary>
        /// <param name="batch"></param>
        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, TopLeftCorner, Color.White);
            //batch.Draw(Image, new Vector2(CollisionBounds.X, CollisionBounds.Y), Color.White);

            //draw the arrow if the player is grounded and able to move.
            if (state == PlayerState.Grounded)
            {
                batch.Draw(DirectionVectorImage, Position, null, Color.White, DirectionVector.X > 0 ? MathF.Asin(DirectionVector.Y) : MathF.PI - MathF.Asin(DirectionVector.Y), new Vector2(0, DirectionVectorImage.Height*0.5f), Vector2.One, SpriteEffects.None, 1);
            }
            
        }


        /// <summary>
        /// Whether the player collides with another collidable.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool CollidesWith(ICollidable other)
        {
            return CollisionBounds.Intersects(other.CollisionBounds);
        }
    }
}
