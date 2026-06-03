using MVVM;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class SettingsView : MonoBehaviour
    {
        [Data("OnCloseClick")] [SerializeField]
        public Button ClosePanelButton;

        [Data("MusicSliderChanged")] [SerializeField]
        public Slider MusicVolumeSlider;

        [Data("SfxSliderChanged")] [SerializeField]
        public Slider SfxVolumeSlider;

        [SerializeField] public GameObject SettingsPanel;

        [Setter("MusicVolume")]
        public float MusicVolume
        {
            set => MusicVolumeSlider.SetValueWithoutNotify(value);
        }

        [Setter("SfxVolume")]
        public float SfxVolume
        {
            set => SfxVolumeSlider.SetValueWithoutNotify(value);
        }

        [Setter("SettingsPanelState")]
        public bool Interactable
        {
            set => SettingsPanel.SetActive(value);
        }
    }
}