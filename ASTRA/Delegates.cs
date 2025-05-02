using ASTRA.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA
{
    /// <summary>
    /// This delegate should be used whenever an item needs to be updated in correspondence with another variable, such as a listener.
    /// </summary>
    internal delegate void UpdateDelegate();

    /// <summary>
    /// This is used inside the Scene Manager to pass along private functions to each scene, so that the scene manager effectively outsources any calls to change scenes.
    /// </summary>
    /// <param name="sceneName"></param>
    internal delegate void SetSceneDelegate(string sceneName);

    /// <summary>
    /// This is used inside the Scene Manager to pass along private functions to each scene, so that the scene can get a reference to another scene for multiscening.
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    internal delegate Scene GetSceneDelegate(string sceneName);

    internal delegate void GameObjectDelegate(GameObject obj);
}
