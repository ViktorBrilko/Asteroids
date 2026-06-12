using Core;
using Core.Audios;
using Core.Signals;
using MVVM;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.ViewModels
{
    public class GameMenuViewModel : IInitializable
    {
        private readonly AudioService _audioService;
        private readonly LoadLevelService _loadLevel;
        private readonly SignalBus _signalBus;
        private readonly WindowsState _windowsState;

        [Data("GameMenuPanelState")] public ReactiveProperty<bool> IsPanelOpen;

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
            _audioService.PlaySfx(_audioService.Config.UI_Click);
            _windowsState.IsGameMenuOpen.Value = false;
            _signalBus.Fire(new PanelChangeStateSignal(false));
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