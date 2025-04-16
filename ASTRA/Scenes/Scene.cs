using ASTRA.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA.Scenes
{
    internal class Scene
    {
        /// <summary>
        /// The list of game objects that are within this scene.
        /// </summary>
        private List<GameObject> GameObjects;

        /// <summary>
        /// The collidables that are contained within this scene.
        /// </summary>
        private List<ICollidable> Collidables;

        /// <summary>
        /// The drawables that are contains within this scene.
        /// </summary>
        private List<IDrawable> Drawables;

        /// <summary>
        /// The player object.
        /// </summary>
        private Player? Player;

        /// <summary>
        /// The UI present during this scene.
        /// </summary>
        internal UI UI;

        /// <summary>
        /// Creates a new scene and initializes all the storage variables.
        /// </summary>
        internal Scene()
        {
            GameObjects = new List<GameObject>();
            Collidables = new List<ICollidable>();
            Drawables = new List<IDrawable>();
            UI = new UI();
        }


        /// <summary>
        /// Adds in a game object.
        /// </summary>
        /// <param name="obj"></param>
        internal void Add(GameObject obj)
        {
            GameObjects.Add(obj);

            if (obj is ICollidable collidable)
            {
                Collidables.Add(collidable);
            }

            if (obj is IDrawable drawable)
            {
                Drawables.Add(drawable);
            }

            if (obj is Player p && Player == null)
            {
                Player = p;
            }
        }

        /// <summary>
        /// Removes an object from the game objects.
        /// </summary>
        /// <param name="obj"></param>
        internal void Remove(GameObject obj)
        {

            ArgumentNullException.ThrowIfNull(obj);


            GameObjects.Remove(obj);

            if (obj is ICollidable collidable && obj is not ASTRA.Player)
            {
                Collidables.Remove(collidable);
            }

            if (obj is IDrawable drawable)
            {
                Drawables.Remove(drawable);
            }

            if (obj is Player p)
            {
                Player = null;
            }
        }

        /// <summary>
        /// Draws out scene.
        /// </summary>
        /// <param name="batch"></param>
        internal void Draw(SpriteBatch batch)
        {
            for (int i = 0; i < Drawables.Count; i++)
            {
                Drawables[i].Draw(batch);
            }
        }

        internal void Update(GameTime gameTime)
        {
            

            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Update(gameTime);
            }

            //only the player collides with the level for now, so if no player is present there's no more logic to go through
            if (Player == null)
                return;

            for (int i = 0; i < Collidables.Count; i++)
            {
                if (Player.CollidesWith(Collidables[i]))
                {
                    Player.Collide(Collidables[i]);
                }
            }



        }









    }
}
