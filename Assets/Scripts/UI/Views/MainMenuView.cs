using MVVM;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class MainMenuView : MonoBehaviour
    {
        [Data("OnPlayClick")] 
        [SerializeField] public Button StartGameButton;
    
        [Data("OnSettingsClick")] 
        [SerializeField] public Button SettingsButton;
        
        [SerializeField] public GameObject MenuPanel;
        
        [Setter("MainMenuPanelState")]
        public bool Interactable
        {
            set => MenuPanel.SetActive(value);
        }
    }
}
