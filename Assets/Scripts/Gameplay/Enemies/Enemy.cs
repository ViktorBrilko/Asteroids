using Core.Audios;
using Core.Configs;
using Cysharp.Threading.Tasks;
using Gameplay.Base;
using Gameplay.Players;
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
        protected void Construct(SignalBus signalBus, AudioService audioService)
        {
            SignalBus = signalBus;
            _audioService = audioService;
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
        
        protected void OnCollisionEnter2D(Collision2D other)
        {
            CollideWithPlayer(other, Config.Damage);
        }

        protected abstract void FireResetSignal();

        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
            _enemyMovement = GetComponent<EnemyMovement>();
            
            _healthComponent.Init(Config.Health);
            _enemyMovement.Init(Config.MoveSpeed, Config.AfterCollisionSpeed, Config.CollisionEffectTime,
                Config.RotationSpeed);
        }

        private void CollideWithPlayer(Collision2D other, int damage)
        {
            if (!other.gameObject.TryGetComponent(out Player player)) return;
            
            _audioService.PlayCollision();
            _enemyMovement.ChangeMoveDirection((transform.position - other.transform.position).normalized);
            _enemyMovement.ChangeMoveSpeed().Forget();
            _enemyMovement.RotateAfterCollision().Forget();

            SignalBus.Fire(new PlayerCollidedSignal(gameObject));
        }
    }
}