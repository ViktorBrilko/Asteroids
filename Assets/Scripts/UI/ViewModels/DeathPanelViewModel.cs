using System;
using Core;
using Core.Audios;
using Core.Signals;
using MVVM;
using UniRx;
using Zenject;

namespace UI.ViewModels
{
    public class DeathPanelViewModel : IInitializable, IDisposable
    {
        public readonly LoadLevelService LoadLevel;
        [Data("DeathPanelState")] public readonly ReactiveProperty<bool> IsPlayerDead = new();
        
        private readonly AudioService _audioService;
        private readonly SignalBus _signalBus;

        public DeathPanelViewModel(LoadLevelService loadLevel, AudioService audioService,
            SignalBus signalBus)
        {
            _audioService = audioService;
            LoadLevel = loadLevel;
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

        [Method("OnMenuClick")]
        public void OnMenuClicked()
        {
            _audioService.PlayUiClick();
            LoadLevel.LoadMenu();
            _signalBus.Fire(new PauseGameSignal(false));
        }

        [Method("OnTryAgainClick")]
        public void OnTryAgainClick()
        {
            _audioService.PlayUiClick();
            LoadLevel.LoadLevel();
            _signalBus.Fire(new PauseGameSignal(false));
        }
        
        private void OnPlayerDied()
        {
            IsPlayerDead.Value = true;
            _signalBus.Fire(new PanelChangeStateSignal(true));
            _signalBus.Fire(new PauseGameSignal(true));
        }
    }
}