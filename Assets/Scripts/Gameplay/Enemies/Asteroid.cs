using Core.Configs;
using Gameplay.Base;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemies
{
    public class Asteroid : Enemy, IDieable, IResetable
    {
        private AsteroidConfig _config;

        [Inject]
        public void Construct(SignalBus signalBus, AsteroidConfig config)
        {
            base.Construct(signalBus);
            _config = config;

            HealthService.Init(_config.Health);
            EnemyMove.Init(config.MoveSpeed, config.AfterCollisionSpeed, config.CollisionEffectTime);
            
            EnemyType = EnemyTypes.Asteroid;
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

        public void Die()
        {
            SignalBus.Fire(new ResetSignal<Asteroid>(this));
            SignalBus.Fire(new EnemyDiedSignal(this, transform.position));
        }
    }
}