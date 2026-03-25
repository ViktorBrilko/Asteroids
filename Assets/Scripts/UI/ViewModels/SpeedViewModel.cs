using System;
using Gameplay.Players;
using MVVM;
using UniRx;
using Zenject;

namespace UI.ViewModels
{
    public class SpeedViewModel : IInitializable, IDisposable
    {
        public readonly PlayerMovement PlayerMovementLogic;
        [Data("Speed")]
        public readonly  ReactiveProperty<string> _currentSpeed = new();

        public SpeedViewModel(PlayerMovement playerMovementLogic)
        {
            PlayerMovementLogic = playerMovementLogic;
        }

        public void Initialize()
        {
            OnSpeedChanged();
            PlayerMovementLogic.OnSpeedChanged += OnSpeedChanged;
        }

        public void Dispose()
        {
            PlayerMovementLogic.OnSpeedChanged -= OnSpeedChanged;
        }
        
        private void OnSpeedChanged(float speed = 0)
        {
            _currentSpeed.Value = "Speed " + speed.ToString("F1");
        }
    }
}
