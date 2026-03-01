using Cysharp.Threading.Tasks;
using Gameplay.Base;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transform _projectileSpawnPoint;
        [SerializeField] private GameObject _laser;
        [SerializeField] private ParticleSystem _shield;

        private const string PLAYER_LAYER = "Player";
        private const string PLAYER_UNCONTROLLABLE_LAYER = "Uncontrollable Player";

        private PlayerConfig _config;
        private Spawner<Bullet> _bulletSpawner;
        private Animator _laserAnimator;
        private bool _canShootBullets = true;
        private int _laserShootsCount;
        private bool _isUncontrollable;
        private bool _isLaserCharging;
        private SignalBus _signalBus;
        private static readonly int IsShooting = Animator.StringToHash("IsShooting");
        private HealthService _healthService;

        public HealthService HealthService => _healthService;
        public bool IsUncontrollable => _isUncontrollable;

        [Inject]
        public void Construct(PlayerConfig config, Spawner<Bullet> bulletSpawner, SignalBus signalBus)
        {
            transform.SetParent(null);
            _config = config;
            _bulletSpawner = bulletSpawner;
            _laserShootsCount = config.LaserCount;
            _signalBus = signalBus;

            _healthService = GetComponent<HealthService>();
            _healthService.Init(_config.Health);
        }

        private void Start()
        {
            _laserAnimator = _laser.GetComponent<Animator>();
        }

        public void OnEnable()
        {
            _signalBus.Subscribe<PlayerCollidedSignal>(OnPlayerCollision);
            _healthService.OnDied += Die;
        }

        public void OnDisable()
        {
            _signalBus.Unsubscribe<PlayerCollidedSignal>(OnPlayerCollision);
            _healthService.OnDied -= Die;
        }

        public void HorizontalMove(float direction)
        {
            transform.Translate(new Vector3(direction, 0, 0) * _config.MoveSpeed * Time.deltaTime);
        }

        public void VerticalMove(float direction)
        {
            transform.Translate(new Vector3(0, direction, 0) * _config.MoveSpeed * Time.deltaTime);
        }

        public void Rotate(bool right)
        {
            if (right)
                transform.Rotate(Vector3.forward * -_config.RotateSpeed * Time.deltaTime);
            else
                transform.Rotate(Vector3.forward * _config.RotateSpeed * Time.deltaTime);
        }

        public async UniTask FireBullets()
        {
            if (_canShootBullets)
            {
                _canShootBullets = false;
                _bulletSpawner.SpawnItem(_projectileSpawnPoint.position, transform.rotation);
                await UniTask.Delay(_config.BulletFireDelay);
                _canShootBullets = true;
            }
        }

        public async void FireLaser()
        {
            if (_laserShootsCount <= 0) return;

            _canShootBullets = false;
            _laser.gameObject.SetActive(true);
            _laserAnimator.SetBool(IsShooting, true);
            await UniTask.Delay(_config.LaserShootingTime);
            _laserAnimator.SetBool(IsShooting, false);
            _laser.gameObject.SetActive(false);
            _canShootBullets = true;

            _laserShootsCount--;
            ChargeLaser();
        }
        
        public void Die()
        {
        }

        private async void ChargeLaser()
        {
            if (_isLaserCharging) return;
            if (_laserShootsCount == _config.LaserCount) return;

            _isLaserCharging = true;
            await UniTask.Delay(_config.LaserCooldown);
            _laserShootsCount++;
            _isLaserCharging = false;

            if (_laserShootsCount != _config.LaserCount)
            {
                ChargeLaser();
            }
        }

        private async void OnPlayerCollision(PlayerCollidedSignal signal)
        {
            float elapsedTime = 0;
            _isUncontrollable = true;
            var direction = (transform.position - signal.CollidedObject.transform.position).normalized;
            gameObject.layer = LayerMask.NameToLayer(PLAYER_UNCONTROLLABLE_LAYER);
            _shield.Play();

            while (elapsedTime < _config.UncontrollableTime)
            {
                MoveAfterCollision(direction);
                elapsedTime += Time.deltaTime;
                await UniTask.NextFrame();
            }

            gameObject.layer = LayerMask.NameToLayer(PLAYER_LAYER);
            _isUncontrollable = false;
            _shield.Stop();
        }

        private void MoveAfterCollision(Vector3 direction)
        {
            transform.Translate(direction * _config.MoveSpeed * Time.deltaTime);
        }
    }
}