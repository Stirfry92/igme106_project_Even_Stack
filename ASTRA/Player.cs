using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private const float MaximumPushSpeed = 8f;     //Represents the maximum speed attainable by pushing.
        private const float MinimumPushSpeed = 3f;      //Represents the minimum speed attainable by pushing.
        private float speed;                            //Represents the speed added to the direction when the player "pushes"
        private float pushChargeTime;                   //Represents the amount of time charging the push.
        private Vector2 velocity;                       //Represents the velocity of the player

        private MouseState currentMState;               //Current state of the mouse (stored for input)
        private MouseState previousMState;              //Previous state of the mouse (stored for input)

        private KeyboardState currentKBState;           //Current state of the keyboard (stored for input)
        private KeyboardState previousKBState;          //Previous state of the keyboard (stored for input)

        private const float TotalTimeToReact = 0.25f;   //Maximum time the player has to react to a collision (seconds)
        private float timeToReact;                      //Time left for the player to react to a collision.

        private Listener<PlayerState> CurrentPlayerState;                      //Current player state.

        internal Listener<int> ThrowableCount { get; }  //Number of throwables the player has.

        /// <summary>
        /// The amount of lives the player has.
        /// </summary>
        internal Listener<int> Lives { get; }

        /// <summary>
        /// The direction in which the player will move.
        /// </summary>
        private Vector2 DirectionVector;

        /// <summary>
        /// The arrow texture that will be used for indicating player movement.
        /// </summary>
        private readonly Texture2D DirectionVectorImage;

        /// <summary>
        /// The event passed from the parent scene to the player in order for the player to handle its own object creation.
        /// </summary>
        internal GameObjectDelegate AddToParent;

        /// <summary>
        /// The event passed from the parent scene to the player in order for the player to further pass references to any object created.
        /// </summary>
        internal GameObjectDelegate RemoveFromParent;

        /// <summary>
        /// The known trowables to the player.
        /// </summary>
        private List<Throwable> KnownThrowables;

        private Color DirectionImageRenderColor = Color.White;

        
        
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
            this.Size = new Vector2(GameDetails.TileSize * 0.8f, GameDetails.TileSize * 0.8f);
            //this.Size = new Vector2(50, 50);
            this.speed = 0f;
            this.velocity = new Vector2(0, 0);
            this.timeToReact = TotalTimeToReact;

            //initialize all the listeners.
            this.CurrentPlayerState = new Listener<PlayerState>(PlayerState.Grounded);
            this.Lives = new Listener<int>(1);

            ThrowableCount = new Listener<int>(2);
            //TODO: This is temporary and should be set to zero for the game.

            KnownThrowables = new List<Throwable>();

            CurrentPlayerState.OnValueChanged += UpdateDirectionColor;
        }

        /// <summary>
        /// Updates the direction image render color.
        /// </summary>
        private void UpdateDirectionColor()
        {
            switch (CurrentPlayerState.Value)
            {
                case PlayerState.Floating:
                    {
                        DirectionImageRenderColor = Color.Blue;
                        break;
                    }

                case PlayerState.Grounded:
                    {
                        DirectionImageRenderColor = Color.White;
                        break;
                    }

                default:
                    {
                        DirectionImageRenderColor = Color.White;
                        break;
                    }
            }
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
        /// The image of the player.
        /// </summary>
        public Texture2D Image { get; }


        internal override void Reset()
        {
            Position = GameDetails.CenterOfScreen;
            velocity = Vector2.Zero;
            CurrentPlayerState.Value = ASTRA.PlayerState.Grounded;

            foreach (Throwable throwable in KnownThrowables)
            {
                RemoveFromParent(throwable);
            }

            KnownThrowables.Clear();

            ThrowableCount.Value = 2;
            Lives.Value = 1;
        }


        /// <summary>
        /// Handles all collisions (without updating the other object's status!).
        /// Should be called by a manager class which checks the collision between the player and all other objects.
        /// This way, the player knows how to react when it collides with something, but does not handle checking collision.
        /// </summary>
        /// <param name="other"></param>
        public void Collide(ICollidable other)
        {
            
            if (other is Throwable t && !t.JustThrown)
            {
                
                //ensure that an object doesn't immediately collide
                Collide(t);
                KnownThrowables.Remove(t);
                
            }
            
            if (other is CollidableWall || other is GameDoor)
            {
                StaticCollision(other);
            }
        }
        /// <summary>
        /// Private method called by base Collide method. Determines behavior for specifcially throwable objects.
        /// </summary>
        /// <param name="other"></param>
        private void Collide(Throwable other)
        {
            ThrowableCount.Value++;
        }

        /// <summary>
        /// Handles all collisions between objects where the player must be knocked back slightly during the collision to get out of the other's collision box.
        /// </summary>
        /// <param name="other"></param>
        private void StaticCollision(ICollidable other)
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
                    CurrentPlayerState.Value = PlayerState.Grounded;
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
                    CurrentPlayerState.Value = PlayerState.Grounded;
                }
            }
            //Instance where the player is above the collidable surface
            else if (intersection.Height <= intersection.Width && CollisionBounds.Y <= other.CollisionBounds.Y)
            {
                Position = new Vector2(Position.X, Position.Y - intersection.Height);
                if (currentKBState.IsKeyDown(Keys.Space))
                {
                    velocity = new Vector2(velocity.X, -velocity.Y);
                }
                else
                {
                    velocity = Vector2.Zero;
                    CurrentPlayerState.Value = PlayerState.Grounded;
                }
            }
            //Instance where the player is below the collidable surface
            else if (intersection.Height <= intersection.Width && CollisionBounds.Y > other.CollisionBounds.Y)
            {
                Position = new Vector2(Position.X, Position.Y + intersection.Height);
                if (currentKBState.IsKeyDown(Keys.Space))
                {
                    velocity = new Vector2(velocity.X, -velocity.Y);
                }
                else
                {
                    velocity = Vector2.Zero;
                    CurrentPlayerState.Value = PlayerState.Grounded;
                }
            }

            //Since the player collided, give them time to react to the collision (push off from the wall).
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
            //First, reduce the amount of time the player has to react to a collision:
            timeToReact -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            switch (CurrentPlayerState.Value)
            {
                case PlayerState.Grounded:
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
                            CurrentPlayerState.Value = PlayerState.Floating;
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
                            CurrentPlayerState.Value = PlayerState.Floating;
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

                    /*
                    //Player throws something
                    if (currentMState.LeftButton == ButtonState.Released && previousMState.LeftButton == ButtonState.Pressed && throwableCount > 0)
                    {
                        //create a new throwable
                        Throwable newThrowable = new Throwable(Position);

                        //Decrement the amount of things the player has to throw:
                        throwableCount--;
                        //Set a direction to where the mouse is pointing.
                        dir = (currentMState.Position.ToVector2()) - Position;
                        dir.Normalize();

                        velocity = velocity - dir * 2;

                        if (timeToReact < 0)
                        {
                            state = PlayerState.Floating;
                            timeToReact = TotalTimeToReact;
                        }
                    }
                    */

                    
                    break;
               
                case PlayerState.Floating:
                    //While the player is floating, we only want them to be able to push if the timeToReact is above zero seconds.
                    if (timeToReact >= 0 && currentKBState.IsKeyUp(Keys.Space) && previousKBState.IsKeyDown(Keys.Space))
                    {
                        //Set a direction to where the mouse is pointing.
                        dir = (currentMState.Position.ToVector2()) - Position;
                        dir.Normalize();

                        //Determine speed and velocity based on how long the spacebar was held. 
                        speed = MathHelper.Clamp(MaximumPushSpeed * pushChargeTime, MinimumPushSpeed, MaximumPushSpeed);
                        velocity = velocity + dir * speed;
                    }
                    //Otherwise, the player can throw.
                    else if (currentMState.LeftButton == ButtonState.Released && previousMState.LeftButton == ButtonState.Pressed && ThrowableCount.Value > 0)
                    {

                        //Set a direction to where the mouse is pointing.
                        dir = (currentMState.Position.ToVector2()) - Position;
                        dir.Normalize();



                        //create the new throwable
                        Throwable newThrowable = new Throwable(Position, dir * velocity.Length());
                        newThrowable.Remove = this.RemoveFromParent;

                        //add the throwable to the parent
                        AddToParent(newThrowable);

                        //add reference to the known throwables
                        KnownThrowables.Add(newThrowable);

                        //Decrement the amount of things the player has to throw:
                        ThrowableCount.Value--;

                        velocity = velocity - dir * 2;
                    }
                    break;
            }

            //Once the input handling is done, apply whatever motion to the player's position here:
            Position = Position + velocity;

            //Perform necessary "clean up" tasks:
            //Set Previous states
            previousKBState = currentKBState;
            previousMState = currentMState;

            //update the throwables
            for (int i = 0; i < KnownThrowables.Count; i++)
            {
                if (KnownThrowables[i].JustThrown && !CollidesWith(KnownThrowables[i]))
                {
                    KnownThrowables[i].JustThrown = false;
                }
            }
            
        }

        /// <summary>
        /// Draws out the asset.
        /// </summary>
        /// <param name="batch"></param>
        public void Draw(SpriteBatch batch)
        {
            //batch.Draw(Image, TopLeftCorner, Color.White);
            batch.Draw(Image, CollisionBounds, Color.White);

            batch.Draw(DirectionVectorImage, Position, null, DirectionImageRenderColor, DirectionVector.X > 0 ? MathF.Asin(DirectionVector.Y) : MathF.PI - MathF.Asin(DirectionVector.Y), new Vector2(0, DirectionVectorImage.Height*0.5f), Vector2.One, SpriteEffects.None, 0);
            
            
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
