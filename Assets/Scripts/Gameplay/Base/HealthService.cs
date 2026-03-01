using System;
using UnityEngine;

namespace Gameplay.Base
{
    public class HealthService : MonoBehaviour
    {
        private int _currentHealth;

        public event Action OnDied; 

        public void Init(int maxHealth)
        {
            _currentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                OnDied?.Invoke();
            }
        }
        
    }
}