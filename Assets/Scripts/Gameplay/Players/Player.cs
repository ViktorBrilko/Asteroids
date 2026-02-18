using System;
using Cysharp.Threading.Tasks;
using Gameplay.Base;
using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    public class Player : MonoBehaviour, IDamagable
    {
        [SerializeField] private Transform _projectileSpawnPoint;

        private PlayerConfig _playerConfig;
        private Spawner<Bullet> _bulletSpawner;
        private int _currentHealth;
        private bool _canShootBullets = true;
        
        public PlayerConfig PlayerConfig => _playerConfig;

        [Inject]
        public void Construct(PlayerConfig playerConfig, Spawner<Bullet> bulletSpawner)
        {
            _playerConfig = playerConfig;
            _bulletSpawner = bulletSpawner;
            _currentHealth = _playerConfig.Health;
        }

        public void MoveAfterCollision(Vector3 direction)
        {
            transform.Translate(direction * _playerConfig.MoveSpeed * Time.deltaTime);
        }

        public void HorizontalMove(float direction)
        {
            transform.Translate(new Vector3(direction, 0, 0) * _playerConfig.MoveSpeed * Time.deltaTime);
        }

        public void VerticalMove(float direction)
        {
            transform.Translate(new Vector3(0, direction, 0) * _playerConfig.MoveSpeed * Time.deltaTime);
        }

        public void Rotate(bool right)
        {
            if (right)
                transform.Rotate(Vector3.forward * -_playerConfig.RotateSpeed * Time.deltaTime);
            else
                transform.Rotate(Vector3.forward * _playerConfig.RotateSpeed * Time.deltaTime);
        }

        public async UniTask FireBullets()
        {
            if (_canShootBullets)
            {
                _canShootBullets = false;
                _bulletSpawner.SpawnItem(_projectileSpawnPoint.position, transform.rotation);
                await UniTask.Delay(_playerConfig.BulletFireDelay);
                _canShootBullets = true;
            }
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            Debug.Log("Игрок получил урон");

            if (_currentHealth <= 0)
            {
            }
        }
    }
}