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
        internal Vector2 StartPosition = GameDetails.CenterOfScreen;
        internal TestScene() : base()
        {
            Player player = new Player(StartPosition);
            player.AddToParent = Add;
            player.RemoveFromParent = Remove;

            //adds an object
            Add(player);

            //Any object that needs to be in this scene should be added using this.Add(object).
            
            //all collisions are handled for you and all updates are handled for you.
            
        }

    }
}
