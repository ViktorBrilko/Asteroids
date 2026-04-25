using System;
using Core;
using Core.Audios;
using Gameplay.Players;
using MVVM;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.ViewModels
{
    public class DeathPanelViewModel : IInitializable, IDisposable
    {
        public readonly Player Player;
        public readonly LoadLevelService LoadLevel;
        
        private AudioService _audioService;

        [Data("Interactable")] public readonly ReactiveProperty<bool> IsPlayerDead = new();

        public DeathPanelViewModel(Player player, LoadLevelService loadLevel, AudioService audioService)
        {
            _audioService = audioService;
            Player = player;
            LoadLevel = loadLevel;
        }

        public void Initialize()
        {
            Player.OnPlayerDied += OnPlayerDied;
        }

        public void Dispose()
        {
            Player.OnPlayerDied -= OnPlayerDied;
        }

        private void OnPlayerDied()
        {
            Time.timeScale = 0;
            IsPlayerDead.Value = true;
        }

        [Method("OnMenuClick")]
        public void OnMenuClicked()
        {
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