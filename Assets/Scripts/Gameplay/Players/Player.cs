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
        private IAnalyticsService _analyticsService;
        private AudioService _audioService;
        private PlayerConfig _config;
        private SignalBus _signalBus;

        public bool IsUncontrollable { get; set; }

        public HealthService HealthService { get; private set; }

        public void OnEnable()
        {
            HealthService.OnDied += Die;
        }

        public void OnDisable()
        {
            HealthService.OnDied -= Die;
        }

        [Inject]
        public void Construct(PlayerConfig config, AudioService audioService, IAnalyticsService analyticsService,
            SignalBus signalBus)
        {
            transform.SetParent(null);
            _config = config;
            _audioService = audioService;
            _analyticsService = analyticsService;
            _signalBus = signalBus;

            HealthService = GetComponent<HealthService>();
            HealthService.Init(_config.Health);
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

        var player = (Player)target;

        if (GUILayout.Button("Убить игрока")) player.Die();
    }
}
#endif