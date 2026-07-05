using Core.Audios;
using Core.Configs;
using Gameplay.Base;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemies
{
    [RequireComponent(typeof(HealthComponent), typeof(EnemyMovement))]
    public abstract class Enemy : MonoBehaviour, IResettable, IKillable
    {
        private AudioService _audioService;
        private EnemyMovement _enemyMovement;
        protected EnemyType EnemyType;
        private HealthComponent _healthComponent;
        protected SignalBus SignalBus;

        public abstract BaseEnemyConfig Config { get; }

        public EnemyType Type => EnemyType;

        [Inject]
        protected void Construct(SignalBus signalBus, AudioService audioService, HealthComponent healthComponent,
            EnemyMovement enemyMovement)
        {
            SignalBus = signalBus;
            _audioService = audioService;
            _healthComponent = healthComponent;
            _enemyMovement = enemyMovement;
        }

        public void OnEnable()
        {
            _healthComponent.OnDied += Die;
        }

        public void OnDisable()
        {
            _healthComponent.OnDied -= Die;
        }

        public void Die()
        {
            _audioService.PlayExplosion();
            FireResetSignal();
            SignalBus.Fire(new EnemyDiedSignal(this, transform.position));
        }

        protected abstract void FireResetSignal();

        private void Awake()
        {
            _healthComponent.Init(Config.Health);
            _enemyMovement.Init(Config.MoveSpeed, Config.AfterCollisionSpeed, Config.CollisionEffectTime,
                Config.RotationSpeed);
        }
    }
}