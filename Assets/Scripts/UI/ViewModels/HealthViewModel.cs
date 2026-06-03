using System;
using Gameplay.Base;
using MVVM;
using UniRx;
using Zenject;

namespace UI.ViewModels
{
    public class HealthViewModel : IInitializable, IDisposable
    {
        [Data("Health")] public readonly ReactiveProperty<int> Health = new();

        public readonly HealthService HealthService;

        public HealthViewModel(HealthService playerHealth)
        {
            HealthService = playerHealth;
        }

        public void Dispose()
        {
            HealthService.OnHealthChanged -= OnHealthChanged;
        }

        public void Initialize()
        {
            Health.Value = HealthService.CurrentHealth;
            HealthService.OnHealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged(int currentHealth)
        {
            Health.Value = currentHealth;
        }
    }
}