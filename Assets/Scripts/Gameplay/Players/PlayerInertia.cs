using System;
using System.Threading;
using Controls;
using Core.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    public class PlayerInertia : MonoBehaviour
    {
        private bool _stopCompensateInertia;
        private float _inertialSpeed;
        private CancellationTokenSource _inertialCts;
        private Vector2 _inertialDirection;
        private PlayerActionCommands _playerActionCommands;
        private Player _player;
        private PlayerConfig _config;

        public float InertialSpeed
        {
            get => _inertialSpeed;
            set => _inertialSpeed = value;
        }

        public Vector2 InertialDirection
        {
            get => _inertialDirection;
            set => _inertialDirection = value;
        }

        public CancellationTokenSource InertialCts => _inertialCts;

        public event Action<float> OnInertiaSpeedChanged;
        
        [Inject]
        public void Construct(PlayerActionCommands playerActionCommands, PlayerConfig config)
        {
            _playerActionCommands = playerActionCommands;
            _config = config;
        }
        
        public async UniTask InertialMove(float currentSpeed)
        {
            if (_inertialCts != null && _inertialSpeed > currentSpeed) return;

            if (!_player.IsUncontrollable)
                _inertialSpeed = currentSpeed;

            InertiaSpeedChanged(_inertialSpeed);

            if (_inertialCts != null)
            {
                _inertialCts.Cancel();
                _inertialCts.Dispose();
            }

            _inertialCts = new CancellationTokenSource();

            try
            {
                while (true)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, _inertialCts.Token);

                    transform.Translate(_inertialDirection *
                                        _inertialSpeed * Time.deltaTime);
                }
            }
            catch (OperationCanceledException e)
            {
                Debug.Log(e.Message);
            }
        }

        public void InertiaSpeedChanged(float newSpeed)
        {
            OnInertiaSpeedChanged?.Invoke(newSpeed);
        }
        
        public async UniTask<bool> CompensateInertia()
        {
            _stopCompensateInertia = false;
            await CompensationInertia();
            if (_stopCompensateInertia) return false;

            if (_inertialDirection.y < 0)
                _inertialDirection.y = 1;
            else
                _inertialDirection.y = -1;

            _inertialDirection.x = 0;

            return true;
        }
        
        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        private void OnEnable()
        {
            _playerActionCommands.StopCompensateInertia += OnStopCompensationInertia;
        }

        private void OnDisable()
        {
            _playerActionCommands.StopCompensateInertia -= OnStopCompensationInertia;
            
            if (_inertialCts != null)
            {
                _inertialCts.Cancel();
                _inertialCts.Dispose();
                _inertialCts = null;
            }
        }
        
        private void OnStopCompensationInertia()
        {
            _stopCompensateInertia = true;
        }
        
        private async UniTask CompensationInertia()
        {
            try
            {
                while (!_stopCompensateInertia && _inertialSpeed != 0)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, _inertialCts.Token);

                    _inertialSpeed = Mathf.MoveTowards(_inertialSpeed, 0f, _config.SpeedChangeStep * Time.deltaTime);
                    InertiaSpeedChanged(_inertialSpeed);
                }
            }
            catch (OperationCanceledException e)
            {
                Debug.Log(e.Message);
            }
        }
    }
}