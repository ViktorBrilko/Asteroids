using Analytics;
using Core;
using Core.Audios;
using Core.Signals;
using Gameplay.Base;
using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    [RequireComponent(typeof(HealthComponent))]
    public class PlayerDeathHandler : MonoBehaviour
    {
        private const string PlayerDieEventName = "player_died";
        private IAnalyticsService _analyticsService;
        private AudioService _audioService;
        private SignalBus _signalBus;

        public HealthComponent HealthComponent { get; private set; }

        [Inject]
        public void Construct(AudioService audioService, IAnalyticsService analyticsService,
            SignalBus signalBus)
        {
            _audioService = audioService;
            _analyticsService = analyticsService;
            _signalBus = signalBus;
        }

        public void OnEnable()
        {
            HealthComponent.OnDied += Die;
            _signalBus.Subscribe<AdClosedSignal>(TurnOnPauseAfterAdClosed);
        }

        public void OnDisable()
        {
            HealthComponent.OnDied -= Die;
            _signalBus.Unsubscribe<AdClosedSignal>(TurnOnPauseAfterAdClosed);
        }

        public void Die()
        {
            _audioService.PlayPlayerDeath();
            _analyticsService.LogEvent(PlayerDieEventName);
            _signalBus.Fire<PlayerDiedSignal>();
        }

        private void Awake()
        {
            HealthComponent = GetComponent<HealthComponent>();
        }

        private void TurnOnPauseAfterAdClosed()
        {
            _signalBus.Fire(new PauseGameSignal(true));
        }
    }
}