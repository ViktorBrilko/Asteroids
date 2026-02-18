using Core.Configs;
using Cysharp.Threading.Tasks;
using Gameplay.Players;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemies.Asteroids
{
    public class Asteroid : MonoBehaviour, IDamagable, IResetable
    {
        private SignalBus _signalBus;
        private int _currentHealth;
        private AsteroidConfig _config;
        private Vector3 _direction;
        private float _moveSpeed;

        [Inject]
        public void Construct(SignalBus signalBus, AsteroidConfig config)
        {
            _signalBus = signalBus;
            _moveSpeed  = config.MoveSpeed;
            _currentHealth = config.Health;
            _config = config;
            _direction = Vector3.up;
        }

        private void Update()
        {
            Move(_direction);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out Player player))
            {
                _direction = (transform.position - other.transform.position).normalized;
                ChangeMoveSpeed();
                
                player.TakeDamage(_config.Damage);
                _signalBus.Fire(new PlayerCollidedSignal(gameObject));
            }
        }
        
        private void Move(Vector3 direction)
        {
            transform.Translate(direction * _moveSpeed * Time.deltaTime,  Space.Self);
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                DestroyAsteroid();
            }
        }

        private async UniTaskVoid ChangeMoveSpeed()
        {
            _moveSpeed = _config.AfterCollisionSpeed;
            await UniTask.Delay(_config.CollisionEffectTime);
            _moveSpeed = _config.MoveSpeed;
        } 

        private void DestroyAsteroid()
        {
            _signalBus.Fire(new ResetSignal<Asteroid>(this));
            _signalBus.Fire(new EnemyDiedSignal(this));
        }

        public void Reset()
        {
        }
    }
}