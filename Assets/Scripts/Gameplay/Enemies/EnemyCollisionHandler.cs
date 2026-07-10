using System;
using Core.Audios;
using Cysharp.Threading.Tasks;
using Gameplay.Players;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemies
{
    public class EnemyCollisionHandler : MonoBehaviour
    {
        private EnemyMovement _enemyMovement;
        private SignalBus _signalBus;
        private AudioService _audioService;

        [Inject]
        protected void Construct(SignalBus signalBus, AudioService audioService,
            EnemyMovement enemyMovement)
        {
            _signalBus = signalBus;
            _audioService = audioService;
            _enemyMovement = enemyMovement;
        }

        protected void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.TryGetComponent(out Player _)) return;

            _audioService.PlayCollision();

            CollideWithPlayer(other).Forget(exception =>
            {
                if (exception is OperationCanceledException)
                    return;

                Debug.LogException(exception);
            });

            _signalBus.Fire(new PlayerCollidedSignal(gameObject));
        }

        private async UniTask CollideWithPlayer(Collision2D other)
        {
            await UniTask.WhenAll(
                _enemyMovement.ChangeMoveDirection((transform.position - other.transform.position).normalized),
                _enemyMovement.ChangeMoveSpeed(),
                _enemyMovement.RotateAfterCollision()
            );
        }
    }
}