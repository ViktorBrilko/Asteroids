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

        [Inject]
        public void Construct(SignalBus signalBus, UfoConfig config)
        {
            base.Construct(signalBus);
            _config = config;

            HealthService.Init(_config.Health);
            EnemyMove.Init(config.MoveSpeed, config.AfterCollisionSpeed, config.CollisionEffectTime, config.RotationSpeed);

            EnemyType = EnemyTypes.Ufo;
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
            SignalBus.Fire(new ResetSignal<Ufo>(this));
            SignalBus.Fire(new EnemyDiedSignal(this, transform.position));
        }
    }
}