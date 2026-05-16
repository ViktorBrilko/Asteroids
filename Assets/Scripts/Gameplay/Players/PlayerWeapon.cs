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
        [SerializeField] private GameObject _laser;
        [SerializeField] private Animator _laserAnimator;
        [SerializeField] private Transform _projectileSpawnPoint;
        [SerializeField] private Player _player;

        private bool _canShootBullets = true;
        private int _laserShootsCount;
        private int _laserCooldown;
        private WeaponConfig _config;
        private Spawner<Bullet> _bulletSpawner;
        private bool _isLaserCharging;
        private static readonly int IsShooting = Animator.StringToHash("IsShooting");
        private AudioService _audioService;
        private PlayerInputHandler _inputHandler;

        public event Action<int> OnLaserCountChanged;
        public event Action<float> OnLaserChargeStarted;

        public int LaserShootsCount => _laserShootsCount;

        [Inject]
        public void Construct(Spawner<Bullet> bulletSpawner, WeaponConfig config, AudioService audioService,
            PlayerInputHandler inputHandler)
        {
            _bulletSpawner = bulletSpawner;
            _laserShootsCount = config.LaserCount;
            _config = config;
            _audioService = audioService;
            _inputHandler = inputHandler;
        }

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
            if (_laserShootsCount <= 0 || _player.IsUncontrollable) return;
            
            _audioService.PlaySfx(_audioService.Config.LaserShoot);

            _canShootBullets = false;
            _laser.gameObject.SetActive(true);
            _laserAnimator.SetBool(IsShooting, true);
            await UniTask.Delay(_config.LaserShootingTime);
            _laserAnimator.SetBool(IsShooting, false);
            _laser.gameObject.SetActive(false);
            _canShootBullets = true;
            _audioService.StopSfx();

            _laserShootsCount--;
            OnLaserCountChanged?.Invoke(_laserShootsCount);
            ChargeLaser().Forget();
        }

        private async UniTaskVoid ChargeLaser()
        {
            if (_isLaserCharging) return;
            if (_laserShootsCount == _config.LaserCount) return;

            _isLaserCharging = true;
            OnLaserChargeStarted?.Invoke(_config.LaserCooldown);
            await UniTask.Delay(TimeSpan.FromSeconds(_config.LaserCooldown));
            _laserShootsCount++;
            OnLaserCountChanged?.Invoke(_laserShootsCount);
            _isLaserCharging = false;

            if (_laserShootsCount != _config.LaserCount)
            {
                ChargeLaser().Forget();
            }
        }
    }
}