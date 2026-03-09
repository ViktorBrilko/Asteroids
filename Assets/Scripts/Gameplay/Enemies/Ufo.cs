using Core.Configs;
using Gameplay.Base;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemies
{
    public class Ufo : Enemy, IResetable, IDieable
    {
        private UfoConfig _config;
        private Transform _player;
        private bool _isChasing;

        public Transform Player
        {
            get => _player;
            set => _player = value;
        }

        [Inject]
        public void Construct(SignalBus signalBus, UfoConfig config)
        {
            base.Construct(signalBus);
            _config = config;
            
            HealthService.Init(_config.Health);
            EnemyMove.Init(config.MoveSpeed, config.AfterCollisionSpeed, config.CollisionEffectTime);
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            CollideWithPlayer(other, _config.Damage);
        }
        
        public void OnEnable()
        {
            HealthService.OnDied += Die;
        }

        public void OnDisable()
        {
            HealthService.OnDied -= Die;
        }

        private void Update()
        {
            if (_isChasing)
            {
                Chasing();
            }
        }
       
        public void StartChasing()
        {
            EnemyMove.CancelRegularMovement();
            _isChasing = true;
        }

        public void StopChasing()
        {
            _isChasing = false;
            EnemyMove.StartRegularMovement();
        }

        private void Chasing()
        {
            transform.position =
                Vector3.MoveTowards(transform.position, _player.transform.position, _config.MoveSpeed * Time.deltaTime);
        }

        public void Die()
        {
            SignalBus.Fire(new ResetSignal<Ufo>(this));
            SignalBus.Fire(new EnemyDiedSignal(this, transform.position));
        }
    }
}