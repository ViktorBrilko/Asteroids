using System;
using Controls;
using Core.Audios;
using Core.Configs;
using Cysharp.Threading.Tasks;
using Gameplay.Base;
using Gameplay.Weapons;
using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    public class PlayerWeapon : MonoBehaviour
    {
        private static readonly int IsShooting = Animator.StringToHash("IsShooting");
        [SerializeField] private GameObject _laser;
        [SerializeField] private Animator _laserAnimator;
        [SerializeField] private Transform _projectileSpawnPoint;
        [SerializeField] private Player _player;
        private AudioService _audioService;
        private Spawner<Bullet> _bulletSpawner;

        private bool _canShootBullets = true;
        private bool _canShootLaser = true;
        private WeaponConfig _config;
        private PlayerInputHandler _inputHandler;
        private bool _isLaserCharging;
        private int _laserCooldown;

        public int LaserShootsCount { get; private set; }

        private void OnEnable()
        {
            _inputHandler.FireBullet += FireBullets;
            _inputHandler.FireLaser += FireLaser;
        }

        private void OnDisable()
        {
            _inputHandler.FireBullet -= FireBullets;
            _inputHandler.FireLaser -= FireLaser;
        }

        public event Action<int> OnLaserCountChanged;
        public event Action<float> OnLaserChargeStarted;

        [Inject]
        public void Construct(Spawner<Bullet> bulletSpawner, WeaponConfig config, AudioService audioService,
            PlayerInputHandler inputHandler)
        {
            _bulletSpawner = bulletSpawner;
            LaserShootsCount = config.LaserCount;
            _config = config;
            _audioService = audioService;
            _inputHandler = inputHandler;
        }

        private async void FireBullets()
        {
            if (_canShootBullets && !_player.IsUncontrollable)
            {
                _audioService.PlaySfx(_audioService.Config.BulletShoot);
                _canShootBullets = false;
                _bulletSpawner.SpawnItem(_projectileSpawnPoint.position, transform.rotation);
                await UniTask.Delay(_config.BulletFireDelay);
                _canShootBullets = true;
            }
        }

        private async void FireLaser()
        {
            if (LaserShootsCount <= 0 || _player.IsUncontrollable) return;
            if (!_canShootLaser) return;

            _audioService.PlaySfx(_audioService.Config.LaserShoot);

            _canShootBullets = false;
            _canShootLaser = false;
            _laser.gameObject.SetActive(true);
            _laserAnimator.SetBool(IsShooting, true);
            await UniTask.Delay(_config.LaserShootingTime);
            _laserAnimator.SetBool(IsShooting, false);
            _laser.gameObject.SetActive(false);
            _canShootBullets = true;
            _canShootLaser = true;
            _audioService.StopSfx();

            LaserShootsCount--;
            OnLaserCountChanged?.Invoke(LaserShootsCount);
            ChargeLaser().Forget();
        }

        private async UniTaskVoid ChargeLaser()
        {
            if (_isLaserCharging) return;
            if (LaserShootsCount == _config.LaserCount) return;

            _isLaserCharging = true;
            OnLaserChargeStarted?.Invoke(_config.LaserCooldown);
            await UniTask.Delay(TimeSpan.FromSeconds(_config.LaserCooldown));
            LaserShootsCount++;
            OnLaserCountChanged?.Invoke(LaserShootsCount);
            _isLaserCharging = false;

            if (LaserShootsCount != _config.LaserCount) ChargeLaser().Forget();
        }
    }
}