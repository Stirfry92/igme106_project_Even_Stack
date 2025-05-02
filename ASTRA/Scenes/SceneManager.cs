using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// The event that should occur when the user wants to exit the game.
        /// </summary>
        private UpdateDelegate OnExit;

        /// <summary>
        /// The scene currently being updated and drawn.
        /// </summary>
        internal Scene CurrentScene { get; private set; }


        internal SceneManager(UpdateDelegate exit)
        {
            LoadedScenes = new Dictionary<string, Scene>();

            OnExit = exit;
            //start off on the home screen

            if (GameDetails.TestingMode)
            {
                CurrentScene = new TestScene();
            }

            //THIS IS OK SINCE WE NEED TO CHANGE BOOL ANYWAYS TO ACCESS
            else
            {
                SetScene(HomeScreen.ID);

            }
            
        }

        /// <summary>
        /// Gets a reference to a scene from the scene manager. If the scene has not been created, it will be created here.
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        private Scene GetScene(string sceneName)
        {
            if (LoadedScenes.TryGetValue(sceneName, out Scene potentialScene))
            {
                return potentialScene;
            }

            //get the scene and set default characteristics.
            Scene newScene = LoadScene(sceneName);

            //this allows for better coupling since now a scene can handle scene change instead of having a reference to the parent inside the scene.
            newScene.SetScene = SetScene;
            newScene.GetScene = GetScene;
            newScene.ExitGame = OnExit;

            LoadedScenes.Add(sceneName, newScene);
            return newScene;
        }

        /// <summary>
        /// Loads a scene from a file.
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        private Scene LoadScene(string sceneName)
        {
            switch (sceneName)
            {
                case HomeScreen.ID:
                    {
                        return new HomeScreen();
                    }

                case GameScreen.ID:
                    {
                        return new GameScreen();
                    }

                case PauseScreen.ID:
                    {
                        return new PauseScreen();
                    }

                case InfoScreen.ID:
                    {
                        return new InfoScreen();
                    }

                case MechanicsContextScreen.ID:
                    {
                        return new MechanicsContextScreen();
                    }

                case WinScreen.ID:
                    {
                        return new WinScreen();
                    }
            }


            return null;
        }
        

        /// <summary>
        /// Sets the next scene for the game.
        /// </summary>
        /// <param name="sceneName"></param>
        private void SetScene(string sceneName)
        {
            CurrentScene = GetScene(sceneName);
            CurrentScene.Load();
            CurrentScene.Reset();

            Debug.WriteLine("Scene has been set to " + sceneName);
        }




    }
}
