using Core.Configs;
using Gameplay.Base;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemies
{
    public class SmallAsteroid : Enemy, IDieable, IResetable
    {
        private SmallAsteroidConfig _config;

        [Inject]
        public void Construct(SignalBus signalBus, SmallAsteroidConfig config)
        {
            base.Construct(signalBus);
            _config = config;
            
            HealthService.Init(_config.Health);
            EnemyMove.Init(config.MoveSpeed, config.AfterCollisionSpeed, config.CollisionEffectTime);
            
            EnemyType = EnemyTypes.SmallAsteroid;
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
            SignalBus.Fire(new ResetSignal<SmallAsteroid>(this));
            SignalBus.Fire(new EnemyDiedSignal(this, transform.position));
        }
    }
}