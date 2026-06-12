using Gameplay.Players;
using UnityEngine;

namespace Gameplay.Enemies
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class UfoScanner : MonoBehaviour
    {
        [SerializeField] private UfoMovement _parentUfo;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, GetComponent<CircleCollider2D>().radius * 2);
        }

#endif

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Player player)) return;

            _parentUfo.Player = player.transform;
            _parentUfo.StartChasing();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Player player)) return;

            _parentUfo.StopChasing();
            _parentUfo.Player = null;
        }
    }
}