using Core.Audios;
using Gameplay.Base;
using Gameplay.Players;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        protected AudioService AudioService;
        protected EnemyMoveService EnemyMove;
        protected EnemyTypes EnemyType;
        protected HealthService HealthService;
        protected SignalBus SignalBus;

        public EnemyTypes Type => EnemyType;

        protected void Construct(SignalBus signalBus, AudioService audioService)
        {
            SignalBus = signalBus;
            AudioService = audioService;

            HealthService = GetComponent<HealthService>();
            EnemyMove = GetComponent<EnemyMoveService>();
        }

        protected void CollideWithPlayer(Collision2D other, int damage)
        {
            if (other.gameObject.TryGetComponent(out Player player))
            {
                AudioService.PlaySfx(AudioService.Config.Collision);
                EnemyMove.ChangeMoveDirection((transform.position - other.transform.position).normalized);
                EnemyMove.ChangeMoveSpeed().Forget();
                EnemyMove.RotateAfterCollision().Forget();

                player.HealthService.TakeDamage(damage);
                SignalBus.Fire(new PlayerCollidedSignal(gameObject));
            }
        }
    }
}