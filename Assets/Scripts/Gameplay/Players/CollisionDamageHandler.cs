using Gameplay.Base;
using Gameplay.Enemies;
using UnityEngine;

namespace Gameplay.Players
{
    [RequireComponent(typeof(HealthComponent))]
    public class CollisionDamageHandler : MonoBehaviour
    {
        private HealthComponent _healthComponent;

        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.TryGetComponent(out Enemy enemy)) return;
            
            _healthComponent.TakeDamage(enemy.Config.Damage);
        }
    }
}