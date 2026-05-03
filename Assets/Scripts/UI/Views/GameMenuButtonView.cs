using MVVM;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class GameMenuButtonView : MonoBehaviour
    {
        [Data("OpenMenuButtonClick")] [SerializeField]
        public Button MenuSceneButton;
    }
}