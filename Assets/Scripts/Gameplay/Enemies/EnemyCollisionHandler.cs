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
            CollideWithPlayer(other);
        }

        private void CollideWithPlayer(Collision2D other)
        {
            if (!other.gameObject.TryGetComponent(out Player _)) return;

            _audioService.PlayCollision();

            try
            {
                _enemyMovement.ChangeMoveDirection((transform.position - other.transform.position).normalized).Forget();
                _enemyMovement.ChangeMoveSpeed().Forget();
                _enemyMovement.RotateAfterCollision().Forget();
            }
            catch (OperationCanceledException e)
            {
                Debug.Log(e.Message);
            }

            _signalBus.Fire(new PlayerCollidedSignal(gameObject));
        }
    }
}