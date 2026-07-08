using System;
using System.Threading;
using Controls;
using Core.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    [RequireComponent(typeof(Player), typeof(PlayerInertia))]
    public class PlayerMovement : MonoBehaviour
    {
        private Player _player;
        private PlayerConfig _config;
        private float _currentSpeed;
        private CancellationTokenSource _speedCts;
        private PlayerActionCommands _playerActionCommands;
        private PlayerInertia _playerInertia;

        public event Action<float> OnSpeedChanged;
        public event Action<float> OnRotationChanged;
        public event Action<Vector2> OnPositionChanged;

        [Inject]
        public void Construct(PlayerConfig config, PlayerActionCommands playerActionCommands)
        {
            _config = config;
            _playerActionCommands = playerActionCommands;
        }

        public async UniTask ChangeSpeed(bool increase)
        {
            if (_player.IsUncontrollable && increase) return;

            if (_speedCts != null)
            {
                _speedCts.Cancel();
                _speedCts.Dispose();
            }

            _speedCts = new CancellationTokenSource();

            while (!Mathf.Approximately(_currentSpeed, _config.MaxSpeed) ||
                   !Mathf.Approximately(_currentSpeed, 0f))
            {
                await UniTask.Yield(PlayerLoopTiming.Update, _speedCts.Token);

                if (increase)
                {
                    _currentSpeed = Mathf.MoveTowards(_currentSpeed, _config.MaxSpeed,
                        _config.SpeedChangeStep * Time.deltaTime);

                    if (_playerInertia.InertialCts != null && _currentSpeed >= _playerInertia.InertialSpeed)
                    {
                        _playerInertia.InertialCts.Cancel();
                        _playerInertia.InertialSpeed = 0;
                        _playerInertia.ChangeInertiaSpeedInUI(_playerInertia.InertialSpeed);
                    }
                }
                else
                {
                    _currentSpeed = Mathf.MoveTowards(_currentSpeed, 0f, _config.SpeedChangeStep * Time.deltaTime);
                }

                OnSpeedChanged?.Invoke(_currentSpeed);
            }
        }

        public void StartInertialMove()
        {
            _playerInertia.InertialMove(_currentSpeed).Forget(exception =>
            {
                if (exception is OperationCanceledException)
                    return;

                Debug.LogException(exception);
            });
        }

        private void Update()
        {
            OnPositionChanged?.Invoke(transform.position);

            if (_player.IsUncontrollable) return;

            if (_playerActionCommands.YDirection != 0)
                Move(new Vector3(0, _playerActionCommands.YDirection, 0));

            if (_playerActionCommands.Rotation != 0) Rotate(_playerActionCommands.Rotation);
        }

        private void Awake()
        {
            _player = GetComponent<Player>();
            _playerInertia = GetComponent<PlayerInertia>();
        }

        private void OnEnable()
        {
            _playerActionCommands.ChangeSpeed += ChangeSpeed;
            _playerActionCommands.StartMovement += OnStartMovement;
            _playerActionCommands.InertialMovement += StartInertialMove;
        }

        private void OnDisable()
        {
            _playerActionCommands.ChangeSpeed -= ChangeSpeed;
            _playerActionCommands.StartMovement -= OnStartMovement;
            _playerActionCommands.InertialMovement -= StartInertialMove;

            if (_speedCts != null)
            {
                _speedCts.Cancel();
                _speedCts.Dispose();
                _speedCts = null;
            }
        }

        private async UniTask OnStartMovement(bool isForward)
        {
            float currentDirection = isForward ? 1 : -1;
            var isSameDirection = Mathf.Approximately(currentDirection, _playerInertia.InertialDirection.y) ||
                                  Mathf.Approximately(0, _playerInertia.InertialDirection.y);

            if (isSameDirection)
            {
                ChangeSpeed(true).Forget(exception =>
                {
                    if (exception is OperationCanceledException)
                        return;

                    Debug.LogException(exception);
                });
            }
            else
            {
                bool wasFinished = await _playerInertia.CompensateInertia();

                if (wasFinished)
                {
                    ChangeSpeed(true).Forget(exception =>
                    {
                        if (exception is OperationCanceledException)
                            return;

                        Debug.LogException(exception);
                    });
                }
            }
        }

        private void Move(Vector3 direction)
        {
            if (_playerInertia.InertialSpeed > _currentSpeed) return;

            transform.Translate(direction.normalized * (_currentSpeed * Time.deltaTime));
            _playerInertia.InertialDirection = new Vector2(0, direction.y);
        }

        private void Rotate(float rotation)
        {
            transform.Rotate(0, 0, rotation * _config.RotationSpeed * Time.deltaTime);
            OnRotationChanged?.Invoke(transform.rotation.eulerAngles.z);
        }
    }
}