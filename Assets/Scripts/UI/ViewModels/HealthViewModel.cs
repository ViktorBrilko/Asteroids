using System;
using Gameplay.Base;
using MVVM;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.ViewModels
{
    public class HealthViewModel : IInitializable, IDisposable
    {
        public readonly HealthService HealthService;
        [Data("Health")]
        public readonly ReactiveProperty<int> Health = new();

        public HealthViewModel(HealthService playerHealth)
        {
            HealthService = playerHealth;
        }

        public void Initialize()
        {
            Health.Value = HealthService.CurrentHealth;
            HealthService.OnHealthChanged += OnHealthChanged;
        }

        public void Dispose()
        {
            HealthService.OnHealthChanged -= OnHealthChanged;
        }
        
        private void OnHealthChanged(int currentHealth)
        {
            Health.Value = currentHealth;
        }
    }
}
