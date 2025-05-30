﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ASTRA.UserInterface
{
    internal abstract class UIComponent : GameObject, IDrawable
    {
        /// <summary>
        /// The ID of the element.
        /// </summary>
        internal readonly string ID;


        /// <summary>
        /// Creates a new UI component.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="position"></param>
        /// <param name="origin"></param>
        internal UIComponent(string ID, Vector2 position, ComponentOrigin origin) : base(position, origin)
        {
            this.ID = ID;
        }

        public Texture2D Image { get; protected set; }

        /// <summary>
        /// Draws the UI Component. As a base, this shouldn't do anything, since in the future a UI element won't need to be drawn?
        /// </summary>
        /// <param name="batch"></param>
        public abstract void Draw(SpriteBatch batch);
        // TODO: NOTE FROM STERLING: For the same reason I commented the code on GameObject (See GameObject's Draw method) I commented out below
        // and uncommented above.
        //public override abstract void Draw(SpriteBatch batch);
    }
}
