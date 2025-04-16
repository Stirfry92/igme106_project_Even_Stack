using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ASTRA
{

    /// <summary>
    /// What is typically considered a game object inside the game.<br></br>
    /// NOTES FROM ZACH: Please take note of <see cref="Size"/> and make sure that gets set in the constructor of the classes implementing GameObject.
    /// </summary>
    internal abstract class GameObject
    {
        /// <summary>
        /// The size of the game object.<br></br>
        /// FROM ZACH: This is being set with the required keyword as this needs to be set in the constructor. If there are errors being thrown about this variable, get rid of the required keyword.
        /// FROM STERLING: I got rid of it :(
        /// </summary>
        internal Vector2 Size { get; set; }

        /// <summary>
        /// The current position of the game object. When rendering, use <see cref="TopLeftCorner"/> instead.
        /// </summary>
        internal Vector2 Position { get; set; }

        /// <summary>
        /// The origin of the game object. This cannot be changed.
        /// </summary>
        internal readonly ComponentOrigin Origin;

        /// <summary>
        /// The top left corner of the game object. Since this cannot be changed, use <see cref="Position"/> to change the game object's position.
        /// </summary>
        internal Vector2 TopLeftCorner
        {
            get
            {
                switch (Origin)
                {

                    
                    case ComponentOrigin.TopLeft:
                        {
                            return Position;
                        }

                    case ComponentOrigin.TopCenter:
                        {
                            return Position - new Vector2(Size.X * 0.5f, 0);
                        }

                    case ComponentOrigin.TopRight:
                        {
                            return Position - new Vector2(Size.X, 0);
                        }

                    case ComponentOrigin.CenterLeft:
                        {
                            return Position - new Vector2(0, Size.Y * 0.5f);
                        }

                    case ComponentOrigin.Center:
                        {
                            return Position - new Vector2(Size.X * 0.5f, Size.Y*0.5f);
                        }

                    case ComponentOrigin.CenterRight:
                        {
                            return Position - new Vector2(Size.X, Size.Y * 0.5f);
                        }

                    case ComponentOrigin.BottomLeft:
                        {
                            return Position - new Vector2(0, Size.Y);
                        }

                    case ComponentOrigin.BottomCenter:
                        {
                            return Position - new Vector2(Size.X * 0.5f, Size.Y);
                        }

                    case ComponentOrigin.BottomRight:
                        {
                            return Position - Size;
                        }
                }

                return Position;
            }
        }

        /// <summary>
        /// Creates a new game object.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="origin"></param>
        internal GameObject(Vector2 position, ComponentOrigin origin)
        {
            Position = position;
            Origin = origin;
        }

        /// <summary>
        /// Updates the game object. Many classes might not use this and might update based on a dependency with another class (like a button and a door).
        /// </summary>
        /// <param name="gameTime"></param>
        internal abstract void Update(GameTime gameTime);

        /// <summary>
        /// Resets the object's attributes.
        /// </summary>
        internal virtual void Reset() { }

        public abstract void Draw(SpriteBatch sb);
    }
}
