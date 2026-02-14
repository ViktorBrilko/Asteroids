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

        [Inject]
        public void Construct(PlayerConfig playerConfig, Spawner<Bullet> bulletSpawner)
        {
            _playerConfig = playerConfig;
            _bulletSpawner = bulletSpawner;
            _currentHealth = _playerConfig.Health;
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

        public void Fire()
        {
            _bulletSpawner.SpawnItem(_projectileSpawnPoint.position, transform.rotation);
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