using System;
using System.Threading;
using Core.Configs;
using Cysharp.Threading.Tasks;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _shield;

        private const string PLAYER_LAYER = "Player";
        private const string PLAYER_UNCONTROLLABLE_LAYER = "Uncontrollable Player";

        private CancellationTokenSource _speedCts;
        private CancellationTokenSource _inertionCts;
        private CancellationTokenSource _collisionCts;

        private float _lastXDirection;
        private float _lastYDirection;
        private float _currentSpeed;
        private PlayerConfig _config;
        private SignalBus _signalBus;
        private bool _isUncontrollable;

        public event Action<float> OnSpeedChanged;
        public event Action<float> OnRotationChanged;
        public event Action<Vector2> OnPositionChanged;

        public bool IsUncontrollable => _isUncontrollable;

        [Inject]
        public void Construct(PlayerConfig config, SignalBus signalBus)
        {
            transform.SetParent(null);
            _config = config;
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<PlayerCollidedSignal>(OnPlayerCollision);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<PlayerCollidedSignal>(OnPlayerCollision);

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

        public async UniTaskVoid ChangeSpeed(bool increase)
        {
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
                    await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: _speedCts.Token);

                    if (increase)
                    {
                        if (_inertionCts != null)
                        {
                            _inertionCts.Cancel();
                        }

                        _currentSpeed = Mathf.MoveTowards(_currentSpeed, _config.MaxSpeed, _config.SpeedChangeStep * Time.deltaTime);
                    }
                    else
                    {
                        _currentSpeed = Mathf.MoveTowards(_currentSpeed, 0f, _config.SpeedChangeStep * Time.deltaTime);
                    }

                    OnSpeedChanged?.Invoke(_currentSpeed);

                    if (Mathf.Approximately(_currentSpeed, _config.MaxSpeed) || Mathf.Approximately(_currentSpeed, 0f)) break;
                }
            }
            catch (OperationCanceledException e)
            {
            }
        }

        public async UniTaskVoid InertialMove()
        {
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
                    await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: _inertionCts.Token);

                    transform.Translate(new Vector3(_lastXDirection, _lastYDirection, 0) *
                                        (_currentSpeed * Time.deltaTime));
                    OnPositionChanged?.Invoke(transform.position);

                    if (_currentSpeed == 0f) break;
                }
            }
            catch (OperationCanceledException e)
            {
            }

            _lastXDirection = 0;
            _lastYDirection = 0;
        }

        public void Move(Vector3 direction)
        {
            transform.Translate(direction.normalized * (_currentSpeed * Time.deltaTime));
            _lastXDirection = direction.x;
            _lastYDirection = direction.y;
            OnPositionChanged?.Invoke(transform.position);
        }

        public void Rotate(float rotation)
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
                _isUncontrollable = true;
                Vector3 direction = (transform.position - signal.CollidedObject.transform.position).normalized;
                gameObject.layer = LayerMask.NameToLayer(PLAYER_UNCONTROLLABLE_LAYER);
                _shield.Play();
                _currentSpeed = _config.AfterCollisionSpeed;
                ChangeSpeed(false).Forget();

                while (elapsedTime < _config.UncontrollableTime)
                {
                    Move(direction);
                    elapsedTime += Time.deltaTime;
                    await UniTask.NextFrame(cancellationToken: _collisionCts.Token);
                }

                _isUncontrollable = false;

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