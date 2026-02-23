using Gameplay.Base;
using UnityEngine;

namespace Gameplay.Weapons
{
    public class Laser : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamagable enemy))
            {
                enemy.Die();
            }
        }
    }
}