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
        public readonly PlayerMovement PlayerMovementLogic;
        [Data("Position")]
        public readonly  ReactiveProperty<string> _currentPosition = new();

        public PlayerPositionViewModel(PlayerMovement playerMovementLogic)
        {
            PlayerMovementLogic = playerMovementLogic;
        }

        public void Initialize()
        {
            OnPositionChanged();
            PlayerMovementLogic.OnPositionChanged += OnPositionChanged;
        }

        public void Dispose()
        {
            PlayerMovementLogic.OnPositionChanged -= OnPositionChanged;
        }
        
        private void OnPositionChanged(Vector2 position = new())
        {
            _currentPosition.Value = "Position " + position;
        }
    }
}
