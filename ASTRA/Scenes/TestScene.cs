using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ASTRA.Scenes
{
    internal class TestScene : Scene
    {
        /// <summary>
        /// lets the button see whare the player is at
        /// </summary>
        public event EventHandler Buttons;
        internal new string ID = "Test Scene";

        /// <summary>
        /// An instance of the level loader.
        /// </summary>
        LevelLoader loader;
        internal TestScene() : base()
        {
            //Any object that needs to be in this scene should be added using this.Add(object).

            //all collisions are handled for you and all updates are handled for you.

            loader = new LevelLoader();
            loader.LoadLevel("..\\..\\..\\DemoLevel.txt", Add, Remove);
            Buttons += loader.button;
        }

        internal override void Update(GameTime gameTime)
        {
            Buttons.Invoke(this, EventArgs.Empty);
            base.Update(gameTime);
            Clean();
        }
    }
}
