using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA
{
    /// <summary>
    /// The component origin. Each different type of origin is described using a pictogram, where # represents the origin placement.
    /// </summary>
    public enum ComponentOrigin
    {

        /// <summary>
        /// #  *  *  <br></br>
        /// *  *  *  <br></br>
        /// *  *  *  
        /// </summary>
        TopLeft,

        /// <summary>
        /// *  #  *  <br></br>
        /// *  *  *  <br></br>
        /// *  *  *  
        /// </summary>
        TopCenter,

        /// <summary>
        /// *  *  #<br></br>
        /// *  *  *  <br></br>
        /// *  *  *  
        /// </summary>
        TopRight,

        /// <summary>
        /// *  *  *  <br></br>
        /// #  *  *  <br></br>
        /// *  *  *  
        /// </summary>
        CenterLeft,

        /// <summary>
        /// *  *  *  <br></br>
        /// *  #  *  <br></br>
        /// *  *  *  
        /// </summary>
        Center,

        /// <summary>
        /// *  *  *  <br></br>
        /// *  *  #<br></br>
        /// *  *  *  
        /// </summary>
        CenterRight,

        /// <summary>
        /// *  *  *  <br></br>
        /// *  *  *<br></br>
        /// #  *  *  
        /// </summary>
        BottomLeft,

        /// <summary>
        /// *  *  *  <br></br>
        /// *  *  *<br></br>
        /// *  #  *  
        /// </summary>
        BottomCenter,

        /// <summary>
        /// *  *  *  <br></br>
        /// *  *  *<br></br>
        /// *  *  #  
        /// </summary>
        BottomRight
    }
}
