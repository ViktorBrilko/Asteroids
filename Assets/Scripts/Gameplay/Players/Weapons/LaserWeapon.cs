using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Gameplay.Players.Weapons
{
    public class LaserWeapon : Weapon
    {
        private static readonly int IsShooting = Animator.StringToHash("IsShooting");
        [SerializeField] private LaserProjectile laserProjectile;
        [SerializeField] private Animator _laserAnimator;
        private bool _isLaserCharging;
        private int _laserCooldown;

        public int LaserShootsCount { get; private set; }

        public event Action<int> OnLaserCountChanged;
        public event Action<float> OnLaserChargeStarted;

        [Inject]
        public void Construct()
        {
            LaserShootsCount = Config.LaserCount;
        }

        protected override async UniTask Fire()
        {
            if (LaserShootsCount <= 0 || Player.IsUncontrollable) return;
            if (!Coordinator.CanShootLaser) return;

            AudioService.PlayLaserShot();

            Coordinator.CanShootBullets = false;
            Coordinator.CanShootLaser = false;
            laserProjectile.gameObject.SetActive(true);
            _laserAnimator.SetBool(IsShooting, true);

            try
            {
                await UniTask.Delay(Config.LaserShootingTime, cancellationToken: this.GetCancellationTokenOnDestroy());
            }
            catch (OperationCanceledException e)
            {
                Debug.Log(e.Message);
                return;
            }

            _laserAnimator.SetBool(IsShooting, false);
            laserProjectile.gameObject.SetActive(false);
            Coordinator.CanShootBullets = true;

            Coordinator.CanShootLaser = true;
            AudioService.StopSfx();

            LaserShootsCount--;
            OnLaserCountChanged?.Invoke(LaserShootsCount);

            ChargeLaser().Forget(exception =>
            {
                if (exception is OperationCanceledException)
                    return;

                Debug.LogException(exception);
            });
        }

        private void OnEnable()
        {
            ActionCommands.FireLaser += Fire;
        }

        private void OnDisable()
        {
            ActionCommands.FireLaser -= Fire;
        }

        private async UniTask ChargeLaser()
        {
            if (_isLaserCharging) return;
            if (LaserShootsCount == Config.LaserCount) return;

            _isLaserCharging = true;
            OnLaserChargeStarted?.Invoke(Config.LaserCooldown);

            await UniTask.Delay(TimeSpan.FromSeconds(Config.LaserCooldown),
                cancellationToken: this.GetCancellationTokenOnDestroy());

            LaserShootsCount++;
            OnLaserCountChanged?.Invoke(LaserShootsCount);
            _isLaserCharging = false;

            if (LaserShootsCount != Config.LaserCount)
            {
                ChargeLaser().Forget(exception =>
                {
                    if (exception is OperationCanceledException)
                        return;

                    Debug.LogException(exception);
                });
            }
        }
    }
}