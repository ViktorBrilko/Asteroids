using MVVM;
using UnityEngine;
using UnityEngine.UI;

public class MenuView : MonoBehaviour
{
    [Data("OnPlayClick")] 
    [SerializeField] public Button StartGameButton;
    
    [Data("OnSettingsClick")] 
    [SerializeField] public Button SettingsButton;
    
    [SerializeField] public GameObject SettingsPanel;
    
    [Setter("SettingsButtonState")]
    public bool Interactable
    {
        set => SettingsPanel.SetActive(value);
    }
}
