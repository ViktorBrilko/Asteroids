using System;
using Gameplay.Players;
using MVVM;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.ViewModels
{
    public class PlayerPositionViewModel : IInitializable, IDisposable
    {
        [Data("Position")] public readonly ReactiveProperty<string> _currentPosition = new();

        public readonly PlayerMovement PlayerMovementLogic;

        public PlayerPositionViewModel(PlayerMovement playerMovementLogic)
        {
            PlayerMovementLogic = playerMovementLogic;
        }

        public void Dispose()
        {
            PlayerMovementLogic.OnPositionChanged -= OnPositionChanged;
        }

        public void Initialize()
        {
            OnPositionChanged();
            PlayerMovementLogic.OnPositionChanged += OnPositionChanged;
        }

        private void OnPositionChanged(Vector2 position = new())
        {
            _currentPosition.Value = "Position " + position;
        }
    }
}