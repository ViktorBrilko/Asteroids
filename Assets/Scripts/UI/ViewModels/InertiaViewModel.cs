using System;
using Gameplay.Players;
using MVVM;
using UniRx;
using Zenject;

namespace UI.ViewModels
{
    public class InertiaViewModel : IInitializable, IDisposable
    {
        [Data("InertiaSpeed")] public readonly ReactiveProperty<string> InertialSpeed = new();
        public readonly PlayerInertia PlayerInertia;

        public InertiaViewModel(PlayerInertia playerInertia)
        {
            PlayerInertia = playerInertia;
        }

        public void Dispose()
        {
            PlayerInertia.OnInertiaSpeedChanged -= OnInertiaSpeedChanged;
        }

        public void Initialize()
        {
            OnInertiaSpeedChanged();
            PlayerInertia.OnInertiaSpeedChanged += OnInertiaSpeedChanged;
        }
        
        private void OnInertiaSpeedChanged(float speed = 0)
        {
            InertialSpeed.Value = "Inertial Speed " + speed.ToString("F1");
        }
    }
}