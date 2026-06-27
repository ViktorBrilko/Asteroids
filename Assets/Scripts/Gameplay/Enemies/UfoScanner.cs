using Gameplay.Players;
using UnityEngine;

namespace Gameplay.Enemies
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class UfoScanner : MonoBehaviour
    {
        [SerializeField] private UfoMovement _parentUfo;

#if UNITY_EDITOR

        private CircleCollider2D _collider2D;
        
        private void Start()
        {
            _collider2D  = GetComponent<CircleCollider2D>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _collider2D.radius * 2);
        }
#endif

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Player player)) return;

            _parentUfo.SetTarget(player.transform);
            _parentUfo.StartChasing();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Player _)) return;

            _parentUfo.StopChasing();
            _parentUfo.SetTarget(null);
        }
    }
}