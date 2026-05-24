using System;
using Analytics;
using Core.Audios;
using Core.Configs;
using Core.Signals;
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
        private AudioService _audioService;
        private bool _isUncontrollable;
        private IAnalyticsService _analyticsService;
        private SignalBus _signalBus;

        public bool IsUncontrollable
        {
            get => _isUncontrollable;
            set => _isUncontrollable = value;
        }

        public HealthService HealthService => _healthService;

        [Inject]
        public void Construct(PlayerConfig config, AudioService audioService, IAnalyticsService analyticsService,
            SignalBus signalBus)
        {
            transform.SetParent(null);
            _config = config;
            _audioService = audioService;
            _analyticsService = analyticsService;
            _signalBus = signalBus;

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
            _audioService.PlaySfx(_audioService.Config.PlayerDeath);
            _analyticsService.LogEvent("player_died");        
            _signalBus.Fire<PlayerDiedSignal>();
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