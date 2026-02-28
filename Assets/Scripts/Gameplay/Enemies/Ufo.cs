using Core.Configs;
using Cysharp.Threading.Tasks;
using Gameplay.Base;
using Gameplay.Players;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemies
{

    public class Ufo : MonoBehaviour, IResetable, IDamagable
    {
        private SignalBus _signalBus;
        private UfoConfig _config;
        private int _currentHealth;
        private Vector3 _direction;
        private float _moveSpeed;
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
            _signalBus = signalBus;
            _config = config;
            _currentHealth = config.Health;
            _direction = Vector3.up;
            _moveSpeed = config.MoveSpeed;
        }

        private void Update()
        {
            if (_isChasing)
                Chasing();
            else
                Move(_direction);
        }


        public void Reset()
        {
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

        public void StartChasing()
        {
            _isChasing = true;
        }

        public void StopChasing()
        {
            _isChasing = false;
        }

        private void Chasing()
        {
            transform.position =
                Vector3.MoveTowards(transform.position, _player.transform.position, _moveSpeed * Time.deltaTime);
        }

        private void Move(Vector3 direction)
        {
            transform.Translate(direction * _moveSpeed * Time.deltaTime, Space.Self);
        }

        private async UniTaskVoid ChangeMoveSpeed()
        {
            _moveSpeed = _config.AfterCollisionSpeed;
            await UniTask.Delay(_config.CollisionEffectTime);
            _moveSpeed = _config.MoveSpeed;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            _signalBus.Fire(new ResetSignal<Ufo>(this));
            _signalBus.Fire(new EnemyDiedSignal(this, transform.position));
        }
    }
}