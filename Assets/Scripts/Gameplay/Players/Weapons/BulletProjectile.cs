using Core.Configs;
using Gameplay.Base;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Players.Weapons
{
    public class BulletProjectile : MonoBehaviour, IResettable
    {
        private BulletConfig _bulletConfig;
        private float _currentLifetime;
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(SignalBus signalBus, BulletConfig bulletConfig)
        {
            _signalBus = signalBus;
            _bulletConfig = bulletConfig;
        }

        private void Update()
        {
            Move();
            CheckLifetime();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out HealthComponent enemy))
            {
                enemy.TakeDamage(_bulletConfig.Damage);
                DestroyBullet();
            }
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
            _signalBus.Fire(new ResetSignal<BulletProjectile>(this));
        }
    }
}