using System;
using Core;
using Core.Audios;
using Core.Signals;
using MVVM;
using UniRx;
using Zenject;

namespace UI.ViewModels
{
    public class GameMenuButtonViewModel : IInitializable, IDisposable
    {
        [Data("GameButtonState")] public readonly ReactiveProperty<bool> GameButtonState = new(true);
        
        private readonly AudioService _audioService;
        private readonly SignalBus _signalBus;
        private readonly WindowsState _windowsState;

        public GameMenuButtonViewModel(AudioService audioService, WindowsState windowsState, SignalBus signalBus)
        {
            _audioService = audioService;
            _windowsState = windowsState;
            _signalBus = signalBus;
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        public void Initialize()
        {
            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        [Method("OpenMenuButtonClick")]
        public void OnMenuOpenClicked()
        {
            _audioService.PlayUiClick();
            _windowsState.IsGameMenuOpen.Value = true;
            _signalBus.Fire(new PanelChangeStateSignal(true));
            _signalBus.Fire(new PauseGameSignal(true));
        }

        private void OnPlayerDied()
        {
            GameButtonState.Value = false;
        }
    }
}