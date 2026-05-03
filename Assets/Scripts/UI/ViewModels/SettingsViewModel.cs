using System;
using Core;
using Core.Audios;
using MVVM;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.ViewModels
{
    public class SettingsViewModel : IInitializable, IDisposable
    {
        private Settings _settings;
        private WindowsState _windowsState;
        private AudioService _audioService;

        [Data("SettingsPanelState")] public ReactiveProperty<bool> IsPanelOpen;
        [Data("MusicVolume")] public ReactiveProperty<float> MusicVolume = new();
        [Data("SfxVolume")] public ReactiveProperty<float> SfxVolume = new();

        public SettingsViewModel(Settings settings, WindowsState windowsState, AudioService audioService)
        {
            _settings = settings;
            _windowsState = windowsState;
            IsPanelOpen = _windowsState.IsSettingsOpen;
            _audioService = audioService;
        }

        public void Initialize()
        {
            MusicVolume.Value = _settings.MusicVolume;
            SfxVolume.Value = _settings.SfxVolume;
            
            _settings.SetMusicVolume(MusicVolume.Value);
            _settings.SetSfxVolume(SfxVolume.Value);
        }

        public void Dispose()
        {
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