using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Base;
using Gameplay.Signals;
using Gameplay.Weapons;
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
        private SignalBus _signalBus;
        private HealthService _healthService;
        private CancellationTokenSource _speedCts;
        private CancellationTokenSource _inertionCts;

        //weapon
        private bool _canShootBullets = true;
        private int _laserShootsCount;
        private Spawner<Bullet> _bulletSpawner;
        private Animator _laserAnimator;
        private bool _isLaserCharging;
        private static readonly int IsShooting = Animator.StringToHash("IsShooting");

        private float _lastXDirection;
        private float _lastYDirection;
        private float _currentSpeed;
        private bool _isUncontrollable;

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
            _currentSpeed = _config.MoveSpeed;

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

        public async UniTaskVoid ChangeSpeed(bool increase)
        {
            if (_speedCts != null)
            {
                _speedCts.Cancel();
                _speedCts.Dispose();
            }

            _speedCts = new CancellationTokenSource();
            CancellationToken token = _speedCts.Token;

            try
            {
                while (true)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: token);

                    if (increase)
                    {
                        if (_inertionCts != null)
                        {
                            _inertionCts?.Cancel();
                        }

                        _currentSpeed = Mathf.MoveTowards(_currentSpeed, 10f, _config.SpeedChangeStep * Time.deltaTime);
                        if (_currentSpeed == 10f) break;
                    }
                    else
                    {
                        _currentSpeed = Mathf.MoveTowards(_currentSpeed, 0f, _config.SpeedChangeStep * Time.deltaTime);
                        if (_currentSpeed == 0f) break;
                    }
                }
            }
            catch (OperationCanceledException e)
            {
            }
        }

        public async UniTaskVoid InertialMove()
        {
            _inertionCts = new CancellationTokenSource();
            CancellationToken token = _inertionCts.Token;

            try
            {
                while (true)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: token);

                    transform.Translate(new Vector3(_lastXDirection, _lastYDirection, 0) * _currentSpeed *
                                        Time.deltaTime);

                    if (_currentSpeed == 0f) break;
                }
            }
            catch (OperationCanceledException e)
            {
            }
            finally
            {
                _inertionCts.Dispose();
                _inertionCts = null;
            }

            _lastXDirection = 0;
            _lastYDirection = 0;
        }

        //TODO удалить
        private void OnGUI()
        {
            GUIStyle myStyle = new GUIStyle(GUI.skin.label);
            myStyle.fontSize = 50;
            GUI.Label(new Rect(20, 20, 400, 100), $"Speed: {_currentSpeed}", myStyle);
        }

        public void Move(float xDirection, float yDirection)
        {
            transform.Translate(new Vector3(xDirection, yDirection, 0).normalized * _currentSpeed * Time.deltaTime);
            _lastXDirection = xDirection;
            _lastYDirection = yDirection;
        }

        public void Rotate(float rotation)
        {
            transform.Rotate(0, 0, rotation * _config.RotationSpeed * Time.deltaTime);
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

        private async UniTaskVoid ChargeLaser()
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
            Vector3 direction = (transform.position - signal.CollidedObject.transform.position).normalized;
            gameObject.layer = LayerMask.NameToLayer(PLAYER_UNCONTROLLABLE_LAYER);
            _shield.Play();
            ChangeSpeed(false);

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
            transform.Translate(direction * _config.AfterCollisionSpeed * Time.deltaTime);
        }
    }
}