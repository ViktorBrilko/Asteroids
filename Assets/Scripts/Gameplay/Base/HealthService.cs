using System;
using UnityEngine;

namespace Gameplay.Base
{
    public class HealthService : MonoBehaviour
    {
        private int _currentHealth;

        public int CurrentHealth => _currentHealth;

        public event Action OnDied; 
        public event Action<int> OnHealthChanged;

        public void Init(int maxHealth)
        {
            _currentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            OnHealthChanged?.Invoke(_currentHealth);

            if (_currentHealth <= 0)
            {
                OnDied?.Invoke();
            }
        }
        
    }
}