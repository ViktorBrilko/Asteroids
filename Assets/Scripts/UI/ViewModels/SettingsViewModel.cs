using System;
using Core.Audios;
using MVVM;
using UniRx;
using Zenject;

namespace UI.ViewModels
{
    public class SettingsViewModel : IInitializable, IDisposable
    {
        public Settings Settings;

        private WindowsState _windowsState;
        private AudioService _audioService;

        [Data("SettingsPanelState")] public ReactiveProperty<bool> IsPanelOpen;
        [Data("MusicVolume")] public ReactiveProperty<float> MusicVolume = new();
        [Data("SfxVolume")] public ReactiveProperty<float> SfxVolume = new();

        public SettingsViewModel(Settings settings, WindowsState windowsState, AudioService audioService)
        {
            Settings = settings;
            _windowsState = windowsState;
            IsPanelOpen = _windowsState.IsSettingsOpen;
            _audioService = audioService;
        }

        public void Initialize()
        {
            MusicVolume.Value = Settings.MusicVolume;
            SfxVolume.Value = Settings.SfxVolume;
            
            Settings.SetMusicVolume(MusicVolume.Value);
            Settings.SetSfxVolume(SfxVolume.Value);
        }

        public void Dispose()
        {
        }

        [Method("OnCloseClick")]
        public void OnCloseClicked()
        {
            _audioService.PlaySfx(_audioService.Config.UI_Click);
            _windowsState.IsSettingsOpen.Value = false;
        }

        [Method("MusicSliderChanged")]
        public void OnMusicVolumeChanged(float volume)
        {
            Settings.SetMusicVolume(volume);
        }

        [Method("SfxSliderChanged")]
        public void OnSfxVolumeChanged(float volume)
        {
            Settings.SetSfxVolume(volume);
        }
    }
}