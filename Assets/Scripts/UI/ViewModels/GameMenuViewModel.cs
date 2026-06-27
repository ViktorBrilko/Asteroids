using Core;
using Core.Audios;
using Core.Signals;
using MVVM;
using UniRx;
using Zenject;

namespace UI.ViewModels
{
    public class GameMenuViewModel : IInitializable
    {
        [Data("GameMenuPanelState")] public ReactiveProperty<bool> IsPanelOpen;
        
        private readonly AudioService _audioService;
        private readonly LoadLevelService _loadLevel;
        private readonly SignalBus _signalBus;
        private readonly WindowsState _windowsState;

        public GameMenuViewModel(LoadLevelService loadLevel, AudioService audioService, WindowsState windowsState,
            SignalBus signalBus)
        {
            _audioService = audioService;
            _loadLevel = loadLevel;
            _windowsState = windowsState;
            _signalBus = signalBus;
            IsPanelOpen = _windowsState.IsGameMenuOpen;
        }

        public void Initialize()
        {
            _windowsState.IsGameMenuOpen.Value = false;
        }

        [Method("OnCloseClick")]
        public void OnCloseClicked()
        {
            _audioService.PlayUiClick();
            _windowsState.IsGameMenuOpen.Value = false;
            _signalBus.Fire(new PanelChangeStateSignal(false));
            _signalBus.Fire(new PauseGameSignal(false));
        }

        [Method("OpenMenuSceneClick")]
        public void OnMenuClicked()
        {
            _audioService.PlayUiClick();
            _loadLevel.LoadMenu();
            _signalBus.Fire(new PauseGameSignal(false));
            _windowsState.IsGameMenuOpen.Value = false;
        }

        [Method("OpenSettingsPanelClick")]
        public void OnSettingsClicked()
        {
            _audioService.PlayUiClick();
            _windowsState.IsSettingsOpen.Value = true;
            _windowsState.IsGameMenuOpen.Value = false;
        }
    }
}