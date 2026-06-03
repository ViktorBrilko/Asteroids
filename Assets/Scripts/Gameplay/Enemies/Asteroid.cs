using Core.Audios;
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

        public void OnEnable()
        {
            HealthService.OnDied += Die;
        }

        public void OnDisable()
        {
            HealthService.OnDied -= Die;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            CollideWithPlayer(other, _config.Damage);
        }

        public void Die()
        {
            AudioService.PlaySfx(AudioService.Config.Explosion);
            SignalBus.Fire(new ResetSignal<Asteroid>(this));
            SignalBus.Fire(new EnemyDiedSignal(this, transform.position));
        }

        [Inject]
        public void Construct(SignalBus signalBus, AsteroidConfig config, AudioService audioService)
        {
            base.Construct(signalBus, audioService);
            _config = config;

            HealthService.Init(_config.Health);
            EnemyMove.Init(config.MoveSpeed, config.AfterCollisionSpeed, config.CollisionEffectTime,
                config.RotationSpeed);

            EnemyType = EnemyTypes.Asteroid;
        }
    }
}