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

        public readonly HealthComponent HealthComponent;

        public HealthViewModel(HealthComponent playerHealth)
        {
            HealthComponent = playerHealth;
        }

        public void Dispose()
        {
            HealthComponent.OnHealthChanged -= OnHealthChanged;
        }

        public void Initialize()
        {
            Health.Value = HealthComponent.CurrentHealth;
            HealthComponent.OnHealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged(int currentHealth)
        {
            Health.Value = currentHealth;
        }
    }
}