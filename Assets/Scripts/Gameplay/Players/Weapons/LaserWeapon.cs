using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Gameplay.Players.Weapons
{
    [RequireComponent(typeof(Player))]
    public class LaserWeapon : Weapon
    {
        private static readonly int IsShooting = Animator.StringToHash("IsShooting");
        [SerializeField] private LaserProjectile laserProjectile;
        [SerializeField] private Animator _laserAnimator;
        private bool _isLaserCharging;
        private int _laserCooldown;
        private bool _canShootLaser = true;

        public int LaserShootsCount { get; private set; }

        public event Action<int> OnLaserCountChanged;
        public event Action<float> OnLaserChargeStarted;
        
        [Inject]
        public void Construct()
        {
            LaserShootsCount = Config.LaserCount;
        }

        protected override async void Fire()
        {
            if (LaserShootsCount <= 0 || Player.IsUncontrollable) return;
            if (!_canShootLaser) return;

            AudioService.PlayLaserShot();

            CanShootBullets = false;
            _canShootLaser = false;
            laserProjectile.gameObject.SetActive(true);
            _laserAnimator.SetBool(IsShooting, true);
            await UniTask.Delay(Config.LaserShootingTime);
            _laserAnimator.SetBool(IsShooting, false);
            laserProjectile.gameObject.SetActive(false);
            CanShootBullets = true;
            _canShootLaser = true;
            AudioService.StopSfx();

            LaserShootsCount--;
            OnLaserCountChanged?.Invoke(LaserShootsCount);
            ChargeLaser().Forget();
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
            await UniTask.Delay(TimeSpan.FromSeconds(Config.LaserCooldown));
            LaserShootsCount++;
            OnLaserCountChanged?.Invoke(LaserShootsCount);
            _isLaserCharging = false;

            if (LaserShootsCount != Config.LaserCount) ChargeLaser().Forget();
        }
    }
}