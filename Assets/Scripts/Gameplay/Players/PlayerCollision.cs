using System;
using System.Threading;
using Core.Configs;
using Cysharp.Threading.Tasks;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    [RequireComponent(typeof(Player), typeof(PlayerInertia), typeof(PlayerMovement))]
    public class PlayerCollision : MonoBehaviour
    {
        private const string PlayerLayerName = "Player";
        private const string UncontrollablePlayerLayerName = "Uncontrollable Player";

        private SignalBus _signalBus;
        private PlayerConfig _config;
        private Player _player;
        private PlayerInertia _playerInertia;
        private PlayerMovement _playerMovement;
        private CancellationTokenSource _collisionCts;
        [SerializeField] private ParticleSystem _shield;

        [Inject]
        public void Construct(PlayerConfig config, SignalBus signalBus)
        {
            _config = config;
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _player = GetComponent<Player>();
            _playerInertia = GetComponent<PlayerInertia>();
            _playerMovement = GetComponent<PlayerMovement>();
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<PlayerCollidedSignal>(HandlePlayerCollision);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<PlayerCollidedSignal>(HandlePlayerCollision);

            if (_collisionCts != null)
            {
                _collisionCts.Cancel();
                _collisionCts.Dispose();
                _collisionCts = null;
            }
        }

        private void HandlePlayerCollision(PlayerCollidedSignal signal)
        {
            OnPlayerCollision(signal).Forget(exception =>
            {
                if (exception is OperationCanceledException)
                    return;

                Debug.LogException(exception);
            });
        }

        private async UniTask OnPlayerCollision(PlayerCollidedSignal signal)
        {
            if (_collisionCts != null)
            {
                _collisionCts.Cancel();
                _collisionCts.Dispose();
            }

            _collisionCts = new CancellationTokenSource();

            float elapsedTime = 0;
            _player.IsUncontrollable = true;
            _playerInertia.InertialDirection =
                (transform.position - signal.CollidedObject.transform.position).normalized;
            gameObject.layer = LayerMask.NameToLayer(UncontrollablePlayerLayerName);
            _shield.Play();
            _playerInertia.InertialSpeed = _config.AfterCollisionSpeed;

            _playerMovement.ChangeSpeed(false).Forget(exception =>
            {
                if (exception is OperationCanceledException)
                    return;

                Debug.LogException(exception);
            });

            _playerMovement.StartInertialMove();

            while (elapsedTime < _config.UncontrollableTime)
            {
                elapsedTime += Time.deltaTime;
                await UniTask.NextFrame(_collisionCts.Token);
            }

            _player.IsUncontrollable = false;

            try
            {
                await UniTask.Delay(_config.BeforeShieldStopDelay, cancellationToken: _collisionCts.Token);
            }
            catch (OperationCanceledException e)
            {
                Debug.Log(e.Message);
                return;
            }

            gameObject.layer = LayerMask.NameToLayer(PlayerLayerName);
            _shield.Stop();
        }
    }
}