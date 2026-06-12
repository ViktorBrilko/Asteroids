using Core;
using Core.Audios;
using MVVM;
using UniRx;
using Zenject;

namespace UI.ViewModels
{
    public class SettingsViewModel : IInitializable
    {
        private readonly AudioService _audioService;
        private readonly Settings _settings;
        private readonly WindowsState _windowsState;

        [Data("SettingsPanelState")] public ReactiveProperty<bool> IsPanelOpen;
        [Data("MusicVolume")] public ReactiveProperty<float> MusicVolume = new();
        [Data("SfxVolume")] public ReactiveProperty<float> SfxVolume = new();

        public SettingsViewModel(Settings settings, WindowsState windowsState, AudioService audioService)
        {
            _settings = settings;
            _windowsState = windowsState;
            _audioService = audioService;
            IsPanelOpen = _windowsState.IsSettingsOpen;
        }

        public void Initialize()
        {
            MusicVolume.Value = _settings.MusicVolume;
            SfxVolume.Value = _settings.SfxVolume;

            _settings.SetMusicVolume(MusicVolume.Value);
            _settings.SetSfxVolume(SfxVolume.Value);
        }

        [Method("OnCloseClick")]
        public void OnCloseClicked()
        {
            _audioService.PlaySfx(_audioService.Config.UI_Click);
            _windowsState.IsSettingsOpen.Value = false;
            _windowsState.IsMainMenuOpen.Value = true;
            _windowsState.IsGameMenuOpen.Value = true;
        }

        [Method("MusicSliderChanged")]
        public void OnMusicVolumeChanged(float volume)
        {
            _settings.SetMusicVolume(volume);
            MusicVolume.Value = volume;
        }

        [Method("SfxSliderChanged")]
        public void OnSfxVolumeChanged(float volume)
        {
            _settings.SetSfxVolume(volume);
            SfxVolume.Value = volume;
        }
    }
}