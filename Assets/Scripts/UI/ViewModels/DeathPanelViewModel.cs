using System;
using Core;
using Core.Audios;
using Core.Signals;
using MVVM;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.ViewModels
{
    public class DeathPanelViewModel : IInitializable, IDisposable
    {
        public readonly LoadLevelService LoadLevel;

        private AudioService _audioService;
        private SignalBus _signalBus;

        [Data("Interactable")] public readonly ReactiveProperty<bool> IsPlayerDead = new();

        public DeathPanelViewModel(LoadLevelService loadLevel, AudioService audioService,
            SignalBus signalBus)
        {
            _audioService = audioService;
            LoadLevel = loadLevel;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        private void OnPlayerDied()
        {
            Time.timeScale = 0;
            IsPlayerDead.Value = true;
        }

        [Method("OnMenuClick")]
        public void OnMenuClicked()
        {
            Time.timeScale = 1;
            _audioService.PlaySfx(_audioService.Config.UI_Click);
            LoadLevel.LoadMenu();
        }

        [Method("OnTryAgainClick")]
        public void OnTryAgainClick()
        {
            _audioService.PlaySfx(_audioService.Config.UI_Click);
            LoadLevel.LoadLevel();
            Time.timeScale = 1;
        }
    }
}