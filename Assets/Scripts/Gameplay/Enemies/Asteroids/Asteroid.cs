using System;
using Core.Configs;
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

        [Inject]
        public void Construct(SignalBus signalBus, AsteroidConfig config)
        {
            _signalBus = signalBus;
            _currentHealth = config.Health;
            _config = config;
        }

        private void Update()
        {
            Move();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out Player player))
            {
                player.TakeDamage(_config.Damage);
            }
        }
        
        private void Move()
        {
            transform.Translate(Vector3.up * _config.MoveSpeed * Time.deltaTime,  Space.Self);
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                DestroyAsteroid();
            }
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