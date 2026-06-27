using Gameplay.Base;
using UnityEngine;

namespace Gameplay.Players.Weapons
{
    public class LaserProjectile : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IKillable enemy)) enemy.Die();
        }
    }
}