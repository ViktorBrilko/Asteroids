using System;
using Cysharp.Threading.Tasks;
using Gameplay.Base;
using UnityEngine;
using Zenject;

namespace Gameplay.Players.Weapons
{
    public class BulletWeapon : Weapon
    {
        [SerializeField] private Transform _projectileSpawnPoint;

        private Spawner<BulletProjectile> _bulletSpawner;

        [Inject]
        public void Construct(Spawner<BulletProjectile> bulletSpawner)
        {
            _bulletSpawner = bulletSpawner;
        }

        protected override async UniTask Fire()
        {
            if (Coordinator.CanShootBullets && !Player.IsUncontrollable)
            {
                AudioService.PlayBulletShot();
                Coordinator.CanShootBullets = false;
                _bulletSpawner.SpawnItem(_projectileSpawnPoint.position, transform.rotation);

                try
                {
                    await UniTask.Delay(Config.BulletFireDelay,
                        cancellationToken: this.GetCancellationTokenOnDestroy());
                }
                catch (OperationCanceledException e)
                {
                    Console.WriteLine(e.Message);
                }

                Coordinator.CanShootBullets = true;
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