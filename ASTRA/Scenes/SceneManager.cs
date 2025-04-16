using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA.Scenes
{
    internal class SceneManager
    {

        /// <summary>
        /// The loaded scenes.
        /// </summary>
        private Dictionary<string, Scene> LoadedScenes;

        /// <summary>
        /// The scene currently being updated and drawn.
        /// </summary>
        internal Scene CurrentScene { get; }


        internal SceneManager()
        {
            LoadedScenes = new Dictionary<string, Scene>();
        }

        /// <summary>
        /// Gets a reference to a scene from the scene manager. If the scene has not been created, it will be created here.
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        internal Scene GetScene(string sceneName)
        {
            if (LoadedScenes.TryGetValue(sceneName, out Scene potentialScene))
            {
                return potentialScene;
            }

            Scene newScene = LoadScene(sceneName);

            
        }

        /// <summary>
        /// Loads a scene from a file.
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        private Scene LoadScene(string sceneName)
        {

        }


        /// <summary>
        /// Sets the next scene for the game.
        /// </summary>
        /// <param name="sceneName"></param>
        internal void SetScene(string sceneName)
        {

        }




    }
}
