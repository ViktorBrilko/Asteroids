using System;
using Core.Configs;
using Gameplay.Base;
using Gameplay.Players;
using UnityEngine;
using Zenject;

#if UNITY_EDITOR
using UnityEditor; 
#endif

namespace Gameplay.Players
{
    public class Player : MonoBehaviour
    {
        private PlayerConfig _config;
        private HealthService _healthService;
        
        public event Action OnPlayerDied; 

        public HealthService HealthService => _healthService;

        [Inject]
        public void Construct(PlayerConfig config)
        {
            transform.SetParent(null);
            _config = config;

            _healthService = GetComponent<HealthService>();
            _healthService.Init(_config.Health);
        }

        public void OnEnable()
        {
            _healthService.OnDied += Die;
        }

        public void OnDisable()
        {
            _healthService.OnDied -= Die;
        }
        
        public void Die()
        {
            OnPlayerDied?.Invoke(); 
        }
       
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); 

        Player player = (Player)target;

        if (GUILayout.Button("Убить игрока"))
        {
            player.Die();
        }
    }
}
#endif