using System;
using Gameplay.Base;
using Gameplay.Players;
using UnityEditor;
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

#if UNITY_EDITOR
        public void Heal()
        {
            _currentHealth += 100;
        }
#endif

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


#if UNITY_EDITOR
[CustomEditor(typeof(HealthService))]
public class PlayerHealthEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HealthService player = (HealthService)target;

        if (GUILayout.Button("+100 HP игроку"))
        {
            player.Heal();
        }
    }
}
#endif