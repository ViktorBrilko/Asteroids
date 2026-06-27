using System;
using UnityEngine.SceneManagement;
using Zenject;

namespace Core
{
    public class LoadLevelService : IInitializable, IDisposable
    {
        private const string MenuSceneName = "Menu";
        private const string GameSceneName = "Game";
        
        public event Action OnLoadScene;

        public void Dispose()
        {
            SceneManager.sceneLoaded -= OnAnySceneLoaded;
        }

        public void Initialize()
        {
            OnLoadScene?.Invoke();
            SceneManager.sceneLoaded += OnAnySceneLoaded;
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene(MenuSceneName);
        }

        public void LoadLevel()
        {
            SceneManager.LoadScene(GameSceneName);
        }

        private void OnAnySceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == MenuSceneName) OnLoadScene?.Invoke();
        }
    }
}