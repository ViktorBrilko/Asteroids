using System;
using Gameplay.Base;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Base
{
    public class HealthService : MonoBehaviour
    {
        public int CurrentHealth { get; private set; }

        public event Action OnDied;
        public event Action<int> OnHealthChanged;

        public void Init(int maxHealth)
        {
            CurrentHealth = maxHealth;
        }

#if UNITY_EDITOR
        public void Heal()
        {
            CurrentHealth += 100;
        }
#endif

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            OnHealthChanged?.Invoke(CurrentHealth);

            if (CurrentHealth <= 0) OnDied?.Invoke();
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

        var player = (HealthService)target;

        if (GUILayout.Button("+100 HP игроку")) player.Heal();
    }
}
#endif