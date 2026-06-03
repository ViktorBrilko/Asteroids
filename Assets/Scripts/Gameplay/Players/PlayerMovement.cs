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
        private PlayerConfig _config;
        private float _currentSpeed;
        private float _inertialSpeed;
        private bool _stopCompensateInertion;
        private float _lastYDirection;
        private float _inertionDirection;
        private PlayerInputHandler _playerInputHandler;
        private SignalBus _signalBus;
        private CancellationTokenSource _inertionCts;
        private CancellationTokenSource _speedCts;
        private CancellationTokenSource _collisionCts;

        private void Update()
        {
            Debug.Log(_stopCompensateInertion);

            if (_player.IsUncontrollable) return;

            if (_playerInputHandler.XDirection != 0 || _playerInputHandler.YDirection != 0)
                Move(new Vector3(_playerInputHandler.XDirection, _playerInputHandler.YDirection, 0));

            if (_playerInputHandler.Rotation != 0) Rotate(_playerInputHandler.Rotation);
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<PlayerCollidedSignal>(OnPlayerCollision);
            _playerInputHandler.ChangeSpeed += ChangeSpeed;
            //_playerInputHandler.InertionCompensation += OnCompensateInertionAndSpeedUp;
            _playerInputHandler.StartMovement += OnStartMovement;
            _playerInputHandler.StopCompensateInertion += OnStopCompensationInertion;
            _playerInputHandler.InertialMovement += InertialMove;
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<PlayerCollidedSignal>(OnPlayerCollision);
            _playerInputHandler.ChangeSpeed -= ChangeSpeed;
            //  _playerInputHandler.InertionCompensation -= OnCompensateInertionAndSpeedUp;
            _playerInputHandler.StartMovement -= OnStartMovement;
            _playerInputHandler.StopCompensateInertion -= OnStopCompensationInertion;

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

        private async UniTask ChangeSpeed(bool increase, bool forward = true)
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
                        if (forward)
                        {
                            _currentSpeed = Mathf.MoveTowards(_currentSpeed, _config.MaxForwardSpeed,
                                _config.SpeedChangeStep * Time.deltaTime);
                        }
                        else
                        {
                            _currentSpeed = Mathf.MoveTowards(_currentSpeed, _config.MaxReverseSpeed,
                                _config.SpeedChangeStep * Time.deltaTime);
                        }

                        if (_inertionCts != null && Mathf.Abs(_currentSpeed) >= Mathf.Abs(_inertialSpeed))
                        {
                            Debug.Log("рубим инерцию в ChangeSpeed " + _inertialSpeed);
                            _inertionCts.Cancel();
                            _inertialSpeed = 0;
                        }
                    }
                    else
                    {
                        _currentSpeed = Mathf.MoveTowards(_currentSpeed, 0f, _config.SpeedChangeStep * Time.deltaTime);
                    }

                    OnSpeedChanged?.Invoke(_currentSpeed);

                    if (Mathf.Approximately(_currentSpeed, _config.MaxForwardSpeed) ||
                        Mathf.Approximately(_currentSpeed, _config.MaxReverseSpeed) ||
                        Mathf.Approximately(_currentSpeed, 0f)) break;
                }
            }
            catch (OperationCanceledException e)
            {
            }
        }

        private async UniTask CompensateInertionAndSpeedUp(bool isIncrease, bool isForward)
        {
            _stopCompensateInertion = false;
            await CompensationInertion();
            if (_stopCompensateInertion) return;
            ChangeSpeed(isIncrease, isForward);
        }

        private void OnStopCompensationInertion()
        {
            _stopCompensateInertion = true;
        }

        private async UniTask OnStartMovement(bool isForward)
        {
            float currentDirection = isForward ? 1 : -1;
            bool isSameDirection = Mathf.Approximately(currentDirection, _inertionDirection) ||
                                   Mathf.Approximately(0, _inertionDirection);

            if (isSameDirection)
            {
                Debug.Log("same direction " + currentDirection);
                ChangeSpeed(true, isForward);
            }
            else
            {
                Debug.Log("opposite direction " + currentDirection + ", isForward - " + isForward);
                await CompensateInertionAndSpeedUp(true, isForward);
            }
        }

        private async UniTask CompensationInertion()
        {
            Debug.Log("CompensationInertion - " + _inertialSpeed); 
            try
            {
                while (!_stopCompensateInertion && _inertialSpeed != 0)
                {
                    Debug.Log("CompensationInertion цикл");  
                    await UniTask.Yield(PlayerLoopTiming.Update, _inertionCts.Token);

                    _inertialSpeed = Mathf.MoveTowards(_inertialSpeed, 0f, _config.SpeedChangeStep * Time.deltaTime);
                }
            }
            catch (OperationCanceledException e)
            {
            }
        }

        private async UniTaskVoid InertialMove()
        {
            if (_inertionCts != null && Mathf.Abs(_inertialSpeed) > Mathf.Abs(_currentSpeed)) return;

            _inertialSpeed = _currentSpeed;

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

                    transform.Translate(new Vector3(0, _inertionDirection, 0) *
                                        (Mathf.Abs(_inertialSpeed) * Time.deltaTime));

                    OnPositionChanged?.Invoke(transform.position);
                }
            }
            catch (OperationCanceledException e)
            {
            }
        }

        private void Move(Vector3 direction)
        {
            if (Mathf.Abs(_inertialSpeed) > Mathf.Abs(_currentSpeed)) return;

            transform.Translate(direction.normalized * (Mathf.Abs(_currentSpeed) * Time.deltaTime));
            OnPositionChanged?.Invoke(transform.position);
            _inertionDirection = direction.y;
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
                var direction = (transform.position - signal.CollidedObject.transform.position).normalized;
                gameObject.layer = LayerMask.NameToLayer(PLAYER_UNCONTROLLABLE_LAYER);
                _shield.Play();
                _currentSpeed = _config.AfterCollisionSpeed;
                ChangeSpeed(false);

                while (elapsedTime < _config.UncontrollableTime)
                {
                    Move(direction);
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

        private void OnGUI()
        {
            GUIStyle myStyle = new GUIStyle(GUI.skin.label);
            myStyle.fontSize = 75;
            GUI.Label(new Rect(20, 20, 400, 300), $"InertionSpeed: {_inertialSpeed}", myStyle);
        }
    }
}