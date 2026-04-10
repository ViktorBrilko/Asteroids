using UnityEngine.SceneManagement;

namespace Gameplay.Base
{
    public class LoadLevelService 
    {
        private const string MENU_SCENE_NAME = "Menu";
        private const string GAME_SCENE_NAME = "Game";

        public void LoadMenu()
        {
            SceneManager.LoadScene(MENU_SCENE_NAME);
        }
        
        public void LoadLevel()
        {
            SceneManager.LoadScene(GAME_SCENE_NAME);
        }
    }
}