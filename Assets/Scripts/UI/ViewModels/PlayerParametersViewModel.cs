using System;
using Gameplay.Players;
using MVVM;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.ViewModels
{
    public class PlayerParametersViewModel : IInitializable, IDisposable
    {
        [Data("Position")] public readonly ReactiveProperty<string> CurrentPosition = new();
        [Data("Rotation")] public readonly ReactiveProperty<string> CurrentRotation = new();
        [Data("Speed")] public readonly ReactiveProperty<string> CurrentSpeed = new();

        private readonly PlayerMovement _playerMovementLogic;

        public PlayerParametersViewModel(PlayerMovement playerMovementLogic)
        {
            _playerMovementLogic = playerMovementLogic;
        }

        public void Dispose()
        {
            _playerMovementLogic.OnPositionChanged -= OnPositionChanged;
            _playerMovementLogic.OnRotationChanged -= OnRotationChanged;
            _playerMovementLogic.OnSpeedChanged -= OnSpeedChanged;
        }

        public void Initialize()
        {
            OnPositionChanged();
            _playerMovementLogic.OnPositionChanged += OnPositionChanged;

            OnRotationChanged();
            _playerMovementLogic.OnRotationChanged += OnRotationChanged;

            OnSpeedChanged();
            _playerMovementLogic.OnSpeedChanged += OnSpeedChanged;
        }

        private void OnRotationChanged(float zRotation = 0)
        {
            CurrentRotation.Value = "Rotation " + zRotation.ToString("F1");
        }

        private void OnPositionChanged(Vector2 position = new())
        {
            CurrentPosition.Value = "Position " + position.x.ToString("F0") + ", " + position.y.ToString("F0");
        }

        private void OnSpeedChanged(float speed = 0)
        {
            CurrentSpeed.Value = "Speed " + speed.ToString("F1");
        }
    }
}