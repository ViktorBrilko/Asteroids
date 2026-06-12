using MVVM;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class GameMenuButtonView : MonoBehaviour
    {
        [Data("OpenMenuButtonClick")] [SerializeField]
        public Button MenuSceneButton;

        [Setter("GameButtonState")]
        public bool GameButtonState
        {
            set => MenuSceneButton.interactable = value;
        }
    }
}