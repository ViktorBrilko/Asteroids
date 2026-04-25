using MVVM;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class DeathPanelView : MonoBehaviour
    {
        [Data("OnMenuClick")] 
        [SerializeField] public Button MenuButton;

        [Data("OnTryAgainClick")] 
        [SerializeField] public Button TryAgainButton;
       
        [SerializeField] public GameObject DeathPanel;

        [Setter("Interactable")]
        public bool Interactable
        {
            set => DeathPanel.SetActive(value);
        }
    }
}