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
    //Enum Defined here is utilized in determining player behavior in the main update method.
    public enum PlayerState
    {
        Grounded,
        Floating
    }

    /// <summary>
    /// Represents the player object.
    /// Handles all controls, velocity, movement, and more for the player character.
    /// </summary>
    internal class Player : GameObject, ICollidable, IDrawable
    {
        //Fields:
        private Vector2 dir;                            //Represents the direction the player "faces".
        private const float MaximumPushSpeed = 8f;      //Represents the maximum speed attainable by pushing.
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
            //Local content manager instance
            LocalContentManager lcm = LocalContentManager.Shared;
            Image = lcm.GetTexture("editedAstronaut");

            //Arrow Image is loaded here.
            DirectionVectorImage = lcm.GetTexture("directionTriangle");

            //Determine the stats for the player:
            this.Size = new Vector2(GameDetails.TileSize * 0.8f, GameDetails.TileSize * 0.8f);  //Player is 0.8x A tile (Small boy!)
            this.speed = 0f;
            this.velocity = new Vector2(0, 0);
            this.timeToReact = TotalTimeToReact;

            //Initialize all the listeners here:
            this.CurrentPlayerState = new Listener<PlayerState>(PlayerState.Grounded);
            this.Lives = new Listener<int>(GameDetails.GodMode ? int.MaxValue : 1);

            //Deal with throwables here:
            ThrowableCount = new Listener<int>(2);
            KnownThrowables = new List<Throwable>();

            //Setup Event to help with Arrow UI:
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

        /// <summary>
        /// Resets the player to a "default"
        /// (Since we don't know where the player's "default" position is for a level,
        /// the player's position must later be overwritten).
        /// </summary>
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
                //Ensure that an object doesn't immediately collide
                Collide(t);
                KnownThrowables.Remove(t);
            }

            if (other is CollidableWall || (other is GameDoor d && d.Active.Value))
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
            //Three Phases for Update:

            //1.) Collect Input and Update Initial Requirements:
            //Set current states
            currentKBState = Keyboard.GetState();
            currentMState = Mouse.GetState();

            //Update the direction vector each frame.
            DirectionVector = currentMState.Position.ToVector2() - Position;
            DirectionVector.Normalize();


            //2.) Execute movement:
            //Reduce the amount of time the player has to react to a collision
            timeToReact -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            //Then utilize logic to determine what is applied to velocity and the player status.
            switch (CurrentPlayerState.Value)
            {
                case PlayerState.Grounded:
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
                        else
                        {
                            CurrentPlayerState.Value = PlayerState.Floating;
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

                        //Create the new throwable
                        Throwable newThrowable = new Throwable(Position, dir * velocity.Length());
                        newThrowable.Remove = this.RemoveFromParent;

                        //Add the throwable to the parent
                        AddToParent(newThrowable);

                        //Add reference to the known throwables
                        KnownThrowables.Add(newThrowable);

                        //Decrement the amount of things the player has to throw
                        //and adjust velocity accordingly:
                        ThrowableCount.Value--;
                        velocity = velocity - dir * 2;
                    }
                    break;
            }

            //Once the input handling/logic is complete, apply the changes to the player's position here:
            Position = Position + velocity;

            //Godmode gives an additional check, allowing for teleportation.
            if (GameDetails.GodMode && currentMState.RightButton == ButtonState.Released && previousMState.RightButton == ButtonState.Pressed)
            {
                Position = currentMState.Position.ToVector2();
            }

            //3.) Perform necessary "clean up" tasks:
            //Set Previous states
            previousKBState = currentKBState;
            previousMState = currentMState;

            //Update the throwables
            for (int i = 0; i < KnownThrowables.Count; i++)
            {
                if (KnownThrowables[i].JustThrown && !CollidesWith(KnownThrowables[i]))
                {
                    KnownThrowables[i].JustThrown = false;
                }
            }

            //Kill the player if out of bounds.
            if (!CollisionBounds.Intersects(GameDetails.GameBounds))
            {
                Lives.Value = 0;
            }
        }

        /// <summary>
        /// Draws out the asset.
        /// </summary>
        /// <param name="batch"></param>
        public void Draw(SpriteBatch batch)
        {
            //Character
            batch.Draw(Image, 
                       CollisionBounds, 
                       Color.White);

            //Direction Arrow
            batch.Draw(DirectionVectorImage, 
                       Position, 
                       null, 
                       DirectionImageRenderColor, 
                       DirectionVector.X > 0 ? MathF.Asin(DirectionVector.Y) : MathF.PI - MathF.Asin(DirectionVector.Y), 
                       new Vector2(0, DirectionVectorImage.Height*0.5f), 
                       Vector2.One, 
                       SpriteEffects.None, 
                       0);
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
