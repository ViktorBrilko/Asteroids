using System;
using Gameplay.Players;
using MVVM;
using UniRx;
using Zenject;

namespace UI.ViewModels
{
    public class SpeedViewModel : IInitializable, IDisposable
    {
        [Data("Speed")] public readonly ReactiveProperty<string> _currentSpeed = new();

        public readonly PlayerMovement PlayerMovementLogic;

        public SpeedViewModel(PlayerMovement playerMovementLogic)
        {
            PlayerMovementLogic = playerMovementLogic;
        }

        public void Dispose()
        {
            PlayerMovementLogic.OnSpeedChanged -= OnSpeedChanged;
        }

        public void Initialize()
        {
            OnSpeedChanged();
            PlayerMovementLogic.OnSpeedChanged += OnSpeedChanged;
        }

        private void OnSpeedChanged(float speed = 0)
        {
            _currentSpeed.Value = "Speed " + speed.ToString("F1");
        }
    }
}