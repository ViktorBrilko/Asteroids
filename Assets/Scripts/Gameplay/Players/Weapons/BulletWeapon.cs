using Cysharp.Threading.Tasks;
using Gameplay.Base;
using UnityEngine;
using Zenject;

namespace Gameplay.Players.Weapons
{
    [RequireComponent(typeof(Player))]
    public class BulletWeapon : Weapon
    {
        [SerializeField] private Transform _projectileSpawnPoint;

        private Spawner<BulletProjectile> _bulletSpawner;
        
        [Inject]
        public void Construct(Spawner<BulletProjectile> bulletSpawner)
        {
            _bulletSpawner = bulletSpawner;
        }
        
        protected override async void Fire()
        {
            if (CanShootBullets && !Player.IsUncontrollable)
            {
                AudioService.PlayBulletShot();
                CanShootBullets = false;
                _bulletSpawner.SpawnItem(_projectileSpawnPoint.position, transform.rotation);
                await UniTask.Delay(Config.BulletFireDelay);
                CanShootBullets = true;
            }
        }

        private void OnEnable()
        {
            ActionCommands.FireBullet += Fire;
        }

        private void OnDisable()
        {
            ActionCommands.FireBullet -= Fire;
        }
    }
}