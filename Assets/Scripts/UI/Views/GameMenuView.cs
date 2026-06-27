using MVVM;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class GameMenuView : MonoBehaviour
    {
        [Data("OnCloseClick")] [SerializeField]
        public Button ClosePanelButton;

        [Data("OpenMenuSceneClick")] [SerializeField]
        public Button OpenMenuSceneButton;

        [Data("OpenSettingsPanelClick")] [SerializeField]
        public Button OpenSettingsPanelButton;

        [SerializeField] public GameObject MenuPanel;

        [Setter("GameMenuPanelState")]
        public bool IsOpen
        {
            set => MenuPanel.SetActive(value);
        }
    }
}