using System;
using Core;
using Core.Audios;
using MVVM;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.ViewModels
{
    public class GameMenuViewModel : IInitializable, IDisposable
    {
        private LoadLevelService _loadLevel;
        private WindowsState _windowsState;
        private AudioService _audioService;
        
        [Data("GameMenuPanelState")] public ReactiveProperty<bool> IsPanelOpen;

        public GameMenuViewModel(LoadLevelService loadLevel, AudioService audioService, WindowsState windowsState)
        {
            _audioService = audioService;
            _loadLevel = loadLevel;
            _windowsState = windowsState;
            IsPanelOpen = _windowsState.IsGameMenuOpen;
        }

        public void Initialize()
        {
            _windowsState.IsGameMenuOpen.Value = false;
        }

        public void Dispose()
        {
        }
        
        [Method("OnCloseClick")]
        public void OnCloseClicked()
        {
            _audioService.PlaySfx(_audioService.Config.UI_Click);
            _windowsState.IsGameMenuOpen.Value = false;
            Time.timeScale = 1;
        }

        [Method("OpenMenuSceneClick")]
        public void OnMenuClicked()
        {
            _audioService.PlaySfx(_audioService.Config.UI_Click);
            _loadLevel.LoadMenu();
            Time.timeScale = 1;
            _windowsState.IsGameMenuOpen.Value = false;
        }

        [Method("OpenSettingsPanelClick")]
        public void OnSettingsClicked()
        {
            _audioService.PlaySfx(_audioService.Config.UI_Click);
            _windowsState.IsSettingsOpen.Value = true;
            _windowsState.IsGameMenuOpen.Value = false;
        }
    }
}

