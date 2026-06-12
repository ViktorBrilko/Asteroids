using Core;
using Core.Audios;
using MVVM;
using UniRx;

namespace UI.ViewModels
{
    public class MainMenuViewModel
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