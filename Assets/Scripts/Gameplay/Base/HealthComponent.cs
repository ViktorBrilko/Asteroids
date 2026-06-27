using System;
using UnityEngine;

namespace Gameplay.Base
{
    public class HealthComponent : MonoBehaviour
    {
        public int CurrentHealth { get; private set; }

        public event Action OnDied;
        public event Action<int> OnHealthChanged;

        public void Init(int maxHealth)
        {
            CurrentHealth = maxHealth;
        }

        public void Heal(int amount)
        {
            CurrentHealth += amount;
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            OnHealthChanged?.Invoke(CurrentHealth);

            if (CurrentHealth <= 0) OnDied?.Invoke();
        }
    }
}