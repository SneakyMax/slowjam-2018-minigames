using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class SceneController : MonoBehaviour
    {
        public static SceneController Instance { get; private set; }

        private IList<Scene> loadedScenes;

        private void Awake()
        {
            Instance = this;

            loadedScenes = new List<Scene>();

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                loadedScenes.Add(SceneManager.GetSceneAt(i));
            }
        }

        public void GoTo(Scene scene)
        {
        }
    }
}