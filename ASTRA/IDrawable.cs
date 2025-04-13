using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA
{

    /// <summary>
    /// Represents a renderable component for the game.
    /// </summary>
    public interface IDrawable
    {

        /// <summary>
        /// The image of the IRenderable component.
        /// </summary>
        Texture2D Image { get; }

        /// <summary>
        /// Renders out the Renderable component.
        /// </summary>
        /// <param name="batch"></param>
        void Draw(SpriteBatch batch);
    }
}
