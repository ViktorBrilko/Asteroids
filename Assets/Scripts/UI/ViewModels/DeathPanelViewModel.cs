using System;
using Gameplay.Base;
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

        [Data("Interactable")] public readonly ReactiveProperty<bool> IsPlayerDead = new();

        public DeathPanelViewModel(Player player, LoadLevelService loadLevel)
        {
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
        private void OnMenuClicked()
        {
            LoadLevel.LoadMenu();
        }

        [Method("OnTryAgainClick")]
        public void OnTryAgainClick()
        {
            LoadLevel.LoadLevel();
            Time.timeScale = 1;
        }
    }
}