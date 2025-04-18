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
    internal abstract class Scene
    {
        /// <summary>
        /// The ID of the scene, which should be added to the Scene Manager once the scene has been created.
        /// </summary>
        internal const string ID = "base";


        /// <summary>
        /// This is a delegate for when scenes should be switched. This would be for stuff like:<br></br>
        /// Pressing escape during gameplay to pull up the pause menu.<br></br>
        /// Pressing the start button on the home screen.<br></br>
        /// Pressing the continue button on the pause screen.<br></br>
        /// Stuff like that.
        /// </summary>
        internal SetSceneDelegate SetScene;

        /// <summary>
        /// This is a delegate for when scenes should be referenced, but not changed. This would be for stuff like:<br></br>
        /// The pause menu which hides part of the underlying game.
        /// </summary>
        internal GetSceneDelegate GetScene;

        /// <summary>
        /// Should be called whenever there is logic present to exit the game. This is set inside Scene Manager.
        /// </summary>
        internal UpdateDelegate ExitGame;

        /// <summary>
        /// The list of game objects that are within this scene.
        /// </summary>
        protected List<GameObject> GameObjects;

        /// <summary>
        /// The collidables that are contained within this scene.
        /// </summary>
        protected List<ICollidable> Collidables;

        /// <summary>
        /// The drawables that are contains within this scene.
        /// </summary>
        protected List<IDrawable> Drawables;

        /// <summary>
        /// Objects to remove at the end of every update cycle.
        /// </summary>
        private Queue<GameObject> ObjectsToRemove;

        /// <summary>
        /// The UI present during this scene.
        /// </summary>
        protected UI UI { get; }

        /// <summary>
        /// Creates a new scene and initializes all the storage variables.
        /// </summary>
        protected Scene()
        {
            GameObjects = new List<GameObject>();
            Collidables = new List<ICollidable>();
            Drawables = new List<IDrawable>();
            UI = new UI();

            ObjectsToRemove = new Queue<GameObject>();
        }


        /// <summary>
        /// Adds in a game object.
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void Add(GameObject obj)
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
        }

        /// <summary>
        /// Removes an object from the game objects. This should be called during frame update (collisions, removal of data, etc...)
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void Remove(GameObject obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            ObjectsToRemove.Enqueue(obj);
        }

        /// <summary>
        /// Draws out the scene.
        /// </summary>
        /// <param name="batch"></param>
        internal virtual void Draw(SpriteBatch batch)
        {
            for (int i = 0; i < Drawables.Count; i++)
            {
                Drawables[i].Draw(batch);
            }

            UI.Draw(batch);
        }

        /// <summary>
        /// Updates the scene. This should be overwritten if any logic is required for collisions!<br></br>
        /// If overwriting Update() and need to get rid of objects, please call <see cref="Clean"/>.
        /// </summary>
        /// <param name="gameTime"></param>
        internal virtual void Update(GameTime gameTime)
        {
            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Update(gameTime);

                //TODO: Very bad code right here: This should be shamed and exiled from the project forever.
                //However, we are on a time crunch so it is here for the playtest build because it works.
                if (GameObjects[i] is Player p)
                {
                    
                    foreach (ICollidable collidable in Collidables)
                    {
                        if (p.CollidesWith(collidable) && !p.Equals(collidable))
                        {
                            p.Collide(collidable);
                            collidable.Collide(p);
                        }
                            
                    }
                }
            }

            UI.Update(gameTime);
        }

        /// <summary>
        /// The events that should occur when the scene is loaded. Many scenes will not need to implement this, but the game screen will definitely need to implement this!
        /// </summary>
        internal virtual void Load()
        {}

        /// <summary>
        /// Clears out all objects that should be deleted. This should be called if Update() ever needs to delete objects!
        /// </summary>
        protected void Clean()
        {
            while (ObjectsToRemove.Count > 0)
            {
                GameObject obj = ObjectsToRemove.Dequeue();

                GameObjects.Remove(obj);

                if (obj is ICollidable collidable)
                {
                    Collidables.Remove(collidable);
                }

                if (obj is IDrawable drawable)
                {
                    Drawables.Remove(drawable);
                }
            }
        }

        public override string ToString()
        {
            return ID;
        }
    }
}
