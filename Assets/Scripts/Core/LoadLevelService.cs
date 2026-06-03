using System;
using UnityEngine.SceneManagement;
using Zenject;

namespace Core
{
    public class LoadLevelService : IInitializable, IDisposable
    {
        private const string MENU_SCENE_NAME = "Menu";
        private const string GAME_SCENE_NAME = "Game";

        public void Dispose()
        {
            SceneManager.sceneLoaded -= OnAnySceneLoaded;
        }

        public void Initialize()
        {
            OnLoadScene?.Invoke();

            SceneManager.sceneLoaded += OnAnySceneLoaded;
        }

        public event Action OnLoadScene;

        public void LoadMenu()
        {
            SceneManager.LoadScene(MENU_SCENE_NAME);
        }

        public void LoadLevel()
        {
            SceneManager.LoadScene(GAME_SCENE_NAME);
        }

        private void OnAnySceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == GAME_SCENE_NAME)
                OnLoadScene?.Invoke();
        }
    }
}