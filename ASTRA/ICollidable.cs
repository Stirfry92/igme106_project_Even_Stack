using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA
{

    /// <summary>
    /// Represents an object that can be collided with.
    /// </summary>
    public interface ICollidable
    {

        /// <summary>
        /// The bounds that an object can be collided with. This should be done with respect to the position and size of the component.
        /// <br></br>
        /// Any object that uses this interface should also be inheriting from GameObject.
        /// <br></br>
        /// <br></br>
        /// Example:
        /// <br></br>
        /// <br></br>
        /// <code>
        /// Rectangle CollisionBounds
        /// {
        ///     get
        ///     {
        ///         //return a rectangle that shaves off 5% of all sides of the sprite.
        ///         return 
        ///             new Rectangle(
        ///                             (int)(TopLeftCorner.X + Size.X * 0.05f), 
        ///                             (int)(TopLeftCorner.Y + Size.Y * 0.05f),
        ///                             (int)(Size.X * 0.9f),
        ///                             (int)(Size.Y * 0.9f)
        ///                          );
        ///     }
        /// }
        /// </code>
        /// </summary>
        Rectangle CollisionBounds { get; }

        /// <summary>
        /// Whether this object is colliding with the input collidable.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        bool CollidesWith(ICollidable other);

        /// <summary>
        /// What should occur when a collidable collides with this collidable.
        /// </summary>
        /// <param name="other"></param>
        void Collide(ICollidable other);

        

    }
}
