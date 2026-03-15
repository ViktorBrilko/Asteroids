using Gameplay.Base;
using Gameplay.Players;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        protected SignalBus SignalBus;
        protected EnemyMoveService EnemyMove;
        protected HealthService HealthService;
        protected EnemyTypes EnemyType;
        
        public EnemyTypes Type => EnemyType;

        protected void Construct(SignalBus signalBus)
        {
            SignalBus = signalBus;
            
            HealthService = GetComponent<HealthService>();
            EnemyMove = GetComponent<EnemyMoveService>();
        }

        protected void CollideWithPlayer(Collision2D other, int damage)
        {
            if (other.gameObject.TryGetComponent(out Player player))
            {
                EnemyMove.ChangeMoveDirection((transform.position - other.transform.position).normalized);
                EnemyMove.ChangeMoveSpeed();

                player.HealthService.TakeDamage(damage);
                SignalBus.Fire(new PlayerCollidedSignal(gameObject));
            }
        }
    }
}