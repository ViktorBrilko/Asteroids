using System;
using Core;
using Core.Audios;
using MVVM;
using UniRx;
using Zenject;

namespace UI.ViewModels
{
    public class MainMenuViewModel : IInitializable, IDisposable
    {
        private readonly AudioService _audioService;
        private readonly LoadLevelService _loadLevel;
        private readonly WindowsState _windowsState;

        [Data("MainMenuPanelState")] public ReactiveProperty<bool> IsPanelOpen;

        public MainMenuViewModel(LoadLevelService loadLevel, AudioService audioService, WindowsState windowsState)
        {
            _audioService = audioService;
            _loadLevel = loadLevel;
            _windowsState = windowsState;
            IsPanelOpen = _windowsState.IsMainMenuOpen;
        }

        public void Dispose()
        {
        }

        public void Initialize()
        {
        }

        [Method("OnPlayClick")]
        public void OnPlayClicked()
        {
            _audioService.PlaySfx(_audioService.Config.UI_Click);
            _loadLevel.LoadLevel();
        }

        [Method("OnSettingsClick")]
        public void OnSettingsClicked()
        {
            _audioService.PlaySfx(_audioService.Config.UI_Click);
            _windowsState.IsSettingsOpen.Value = true;
            _windowsState.IsMainMenuOpen.Value = false;
        }
    }
}