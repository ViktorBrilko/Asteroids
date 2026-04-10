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
            _currentSpeed = _config.MoveSpeed;
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
                            _inertionCts?.Cancel();
                        }

                        _currentSpeed = Mathf.MoveTowards(_currentSpeed, 10f, _config.SpeedChangeStep * Time.deltaTime);
                        if (Mathf.Approximately(_currentSpeed, 10f)) break;
                    }
                    else
                    {
                        _currentSpeed = Mathf.MoveTowards(_currentSpeed, 0f, _config.SpeedChangeStep * Time.deltaTime);
                        if (Mathf.Approximately(_currentSpeed, 0f)) break;
                    }

                    OnSpeedChanged?.Invoke(_currentSpeed);
                }
            }
            catch (OperationCanceledException e)
            {
            }
        }

        public async UniTaskVoid InertialMove()
        {
            _inertionCts = new CancellationTokenSource();

            try
            {
                while (true)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: _speedCts.Token);

                    transform.Translate(new Vector3(_lastXDirection, _lastYDirection, 0) * _currentSpeed *
                                        Time.deltaTime);
                    OnPositionChanged?.Invoke(transform.position);

                    if (_currentSpeed == 0f) break;
                }
            }
            catch (OperationCanceledException e)
            {
            }
            finally
            {
                if (_inertionCts != null)
                {
                    _inertionCts.Dispose();
                    _inertionCts = null;
                }
            }

            _lastXDirection = 0;
            _lastYDirection = 0;
        }

        public void Move(Vector3 direction)
        {
            transform.Translate(direction.normalized * _currentSpeed * Time.deltaTime);
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
                await UniTask.NextFrame();
            }

            _isUncontrollable = false;

            await UniTask.Delay(_config.BeforeShieldStopDelay);
            gameObject.layer = LayerMask.NameToLayer(PLAYER_LAYER);
            _shield.Stop();
        }
    }
}