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
        [Data("Position")] public readonly ReactiveProperty<string> _currentPosition = new();
        [Data("Rotation")] public readonly ReactiveProperty<string> _currentRotation = new();
        [Data("Speed")] public readonly ReactiveProperty<string> _currentSpeed = new();
        [Data("InertionSpeed")] public readonly ReactiveProperty<string> _inertionSpeed = new();

        public readonly PlayerMovement PlayerMovementLogic;

        public PlayerParametersViewModel(PlayerMovement playerMovementLogic)
        {
            PlayerMovementLogic = playerMovementLogic;
        }

        public void Dispose()
        {
            PlayerMovementLogic.OnPositionChanged -= OnPositionChanged;
            PlayerMovementLogic.OnRotationChanged -= OnRotationChanged;
            PlayerMovementLogic.OnSpeedChanged -= OnSpeedChanged;
            PlayerMovementLogic.OnInertionSpeedChanged -= OnInertionSpeedChanged;
        }

        public void Initialize()
        {
            OnPositionChanged();
            PlayerMovementLogic.OnPositionChanged += OnPositionChanged;

            OnRotationChanged();
            PlayerMovementLogic.OnRotationChanged += OnRotationChanged;

            OnSpeedChanged();
            PlayerMovementLogic.OnSpeedChanged += OnSpeedChanged;

            OnInertionSpeedChanged();
            PlayerMovementLogic.OnInertionSpeedChanged += OnInertionSpeedChanged;
        }

        private void OnRotationChanged(float zRotation = 0)
        {
            _currentRotation.Value = "Rotation " + zRotation.ToString("F1");
        }

        private void OnPositionChanged(Vector2 position = new())
        {
            _currentPosition.Value = "Position " + position.x.ToString("F1") + ", " + position.y.ToString("F1");
        }

        private void OnSpeedChanged(float speed = 0)
        {
            _currentSpeed.Value = "Speed " + speed.ToString("F1");
        }

        private void OnInertionSpeedChanged(float speed = 0)
        {
            _inertionSpeed.Value = "Inertion Speed " + speed.ToString("F1");
        }
    }
}