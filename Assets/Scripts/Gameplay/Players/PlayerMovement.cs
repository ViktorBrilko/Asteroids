using System;
using System.Threading;
using Controls;
using Core.Configs;
using Cysharp.Threading.Tasks;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    public class PlayerMovement : MonoBehaviour
    {
        private const string PLAYER_LAYER = "Player";
        private const string PLAYER_UNCONTROLLABLE_LAYER = "Uncontrollable Player";
        [SerializeField] private ParticleSystem _shield;
        [SerializeField] private Player _player;
        private CancellationTokenSource _collisionCts;
        private PlayerConfig _config;
        private float _currentSpeed;
        private float _inertialSpeed;
        private CancellationTokenSource _inertionCts;
        private Vector2 _inertionDirection;
        private PlayerInputHandler _playerInputHandler;
        private SignalBus _signalBus;
        private CancellationTokenSource _speedCts;
        private bool _stopCompensateInertion;

        private void Update()
        {
            if (_player.IsUncontrollable) return;

            if (_playerInputHandler.YDirection != 0)
                Move(new Vector3(0, _playerInputHandler.YDirection, 0));

            if (_playerInputHandler.Rotation != 0) Rotate(_playerInputHandler.Rotation);
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<PlayerCollidedSignal>(OnPlayerCollision);
            _playerInputHandler.ChangeSpeed += ChangeSpeed;
            _playerInputHandler.StartMovement += OnStartMovement;
            _playerInputHandler.StopCompensateInertion += OnStopCompensationInertion;
            _playerInputHandler.InertialMovement += InertialMove;
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<PlayerCollidedSignal>(OnPlayerCollision);
            _playerInputHandler.ChangeSpeed -= ChangeSpeed;
            _playerInputHandler.StartMovement -= OnStartMovement;
            _playerInputHandler.StopCompensateInertion -= OnStopCompensationInertion;
            _playerInputHandler.InertialMovement -= InertialMove;

            if (_speedCts != null)
            {
                _speedCts.Cancel();
                _speedCts.Dispose();
                _speedCts = null;
            }

            if (_inertionCts != null)
            {
                _inertionCts.Cancel();
                _inertionCts.Dispose();
                _inertionCts = null;
            }

            if (_collisionCts != null)
            {
                _collisionCts.Cancel();
                _collisionCts.Dispose();
                _collisionCts = null;
            }
        }

        public event Action<float> OnSpeedChanged;
        public event Action<float> OnInertionSpeedChanged;
        public event Action<float> OnRotationChanged;
        public event Action<Vector2> OnPositionChanged;

        [Inject]
        public void Construct(PlayerConfig config, SignalBus signalBus, PlayerInputHandler playerInputHandler)
        {
            transform.SetParent(null);
            _config = config;
            _signalBus = signalBus;
            _playerInputHandler = playerInputHandler;
        }

        private async UniTask ChangeSpeed(bool increase)
        {
            if (_player.IsUncontrollable && increase) return;

            if (_speedCts != null)
            {
                _speedCts.Cancel();
                _speedCts.Dispose();
            }

            _speedCts = new CancellationTokenSource();

            try
            {
                while (true)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, _speedCts.Token);

                    if (increase)
                    {
                        _currentSpeed = Mathf.MoveTowards(_currentSpeed, _config.MaxSpeed,
                            _config.SpeedChangeStep * Time.deltaTime);

                        if (_inertionCts != null && _currentSpeed >= _inertialSpeed)
                        {
                            _inertionCts.Cancel();
                            _inertialSpeed = 0;
                        }
                    }
                    else
                    {
                        _currentSpeed = Mathf.MoveTowards(_currentSpeed, 0f, _config.SpeedChangeStep * Time.deltaTime);
                    }

                    OnSpeedChanged?.Invoke(_currentSpeed);

                    if (Mathf.Approximately(_currentSpeed, _config.MaxSpeed) ||
                        Mathf.Approximately(_currentSpeed, 0f)) break;
                }
            }
            catch (OperationCanceledException e)
            {
            }
        }

        private async UniTask CompensateInertionAndSpeedUp(bool isIncrease)
        {
            _stopCompensateInertion = false;
            await CompensationInertion();
            if (_stopCompensateInertion) return;

            if (_inertionDirection.y < 0)
                _inertionDirection.y = 1;
            else
                _inertionDirection.y = -1;

            _inertionDirection.x = 0;

            ChangeSpeed(isIncrease);
        }

        private void OnStopCompensationInertion()
        {
            _stopCompensateInertion = true;
        }

        private async UniTask OnStartMovement(bool isForward)
        {
            float currentDirection = isForward ? 1 : -1;
            var isSameDirection = Mathf.Approximately(currentDirection, _inertionDirection.y) ||
                                  Mathf.Approximately(0, _inertionDirection.y);

            if (isSameDirection)
                ChangeSpeed(true);
            else
                await CompensateInertionAndSpeedUp(true);
        }

        private async UniTask CompensationInertion()
        {
            try
            {
                while (!_stopCompensateInertion && _inertialSpeed != 0)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, _inertionCts.Token);

                    _inertialSpeed = Mathf.MoveTowards(_inertialSpeed, 0f, _config.SpeedChangeStep * Time.deltaTime);
                    OnInertionSpeedChanged?.Invoke(_inertialSpeed);
                }
            }
            catch (OperationCanceledException e)
            {
            }
        }

        private async UniTaskVoid InertialMove()
        {
            if (_inertionCts != null && _inertialSpeed > _currentSpeed) return;

            if (!_player.IsUncontrollable)
                _inertialSpeed = _currentSpeed;

            OnInertionSpeedChanged?.Invoke(_inertialSpeed);

            if (_inertionCts != null)
            {
                _inertionCts.Cancel();
                _inertionCts.Dispose();
            }

            _inertionCts = new CancellationTokenSource();

            try
            {
                while (true)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, _inertionCts.Token);

                    transform.Translate(_inertionDirection *
                                        _inertialSpeed * Time.deltaTime);

                    OnPositionChanged?.Invoke(transform.position);
                }
            }
            catch (OperationCanceledException e)
            {
            }
        }

        private void Move(Vector3 direction)
        {
            if (_inertialSpeed > _currentSpeed) return;

            transform.Translate(direction.normalized * (_currentSpeed * Time.deltaTime));
            OnPositionChanged?.Invoke(transform.position);
            _inertionDirection.y = direction.y;
        }

        private void Rotate(float rotation)
        {
            transform.Rotate(0, 0, rotation * _config.RotationSpeed * Time.deltaTime);
            OnRotationChanged?.Invoke(transform.rotation.eulerAngles.z);
        }

        private async void OnPlayerCollision(PlayerCollidedSignal signal)
        {
            if (_collisionCts != null)
            {
                _collisionCts.Cancel();
                _collisionCts.Dispose();
            }

            _collisionCts = new CancellationTokenSource();

            try
            {
                float elapsedTime = 0;
                _player.IsUncontrollable = true;
                _inertionDirection = (transform.position - signal.CollidedObject.transform.position).normalized;
                gameObject.layer = LayerMask.NameToLayer(PLAYER_UNCONTROLLABLE_LAYER);
                _shield.Play();
                _inertialSpeed = _config.AfterCollisionSpeed;
                ChangeSpeed(false);
                InertialMove();

                while (elapsedTime < _config.UncontrollableTime)
                {
                    elapsedTime += Time.deltaTime;
                    await UniTask.NextFrame(_collisionCts.Token);
                }

                _player.IsUncontrollable = false;

                await UniTask.Delay(_config.BeforeShieldStopDelay, cancellationToken: _collisionCts.Token);
                gameObject.layer = LayerMask.NameToLayer(PLAYER_LAYER);
                _shield.Stop();
            }
            catch (OperationCanceledException e)
            {
            }
        }
    }
}