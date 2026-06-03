using Core.Configs;
using Gameplay.Base;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons
{
    public class Bullet : MonoBehaviour, IResetable
    {
        private BulletConfig _bulletConfig;
        private float _currentLifetime;
        private SignalBus _signalBus;

        private void Update()
        {
            Move();
            CheckLifetime();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out HealthService enemy))
            {
                enemy.TakeDamage(_bulletConfig.Damage);
                DestroyBullet();
            }
        }

        [Inject]
        public void Construct(SignalBus signalBus, BulletConfig bulletConfig)
        {
            _signalBus = signalBus;
            _bulletConfig = bulletConfig;
        }

        private void CheckLifetime()
        {
            _currentLifetime += Time.deltaTime;
            if (_currentLifetime >= _bulletConfig.MaxLifetime) DestroyBullet();
        }

        private void Move()
        {
            transform.Translate(Vector3.up * _bulletConfig.Speed * Time.deltaTime, Space.Self);
        }

        private void DestroyBullet()
        {
            _currentLifetime = 0;
            _signalBus.Fire(new ResetSignal<Bullet>(this));
        }
    }
}