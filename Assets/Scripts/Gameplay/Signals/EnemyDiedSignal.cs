using Gameplay.Base;
using UnityEngine;

namespace Gameplay.Signals
{
    public class EnemyDiedSignal
    {
        public IDamagable Enemy { get; }
        public Vector3 DeathPosition { get; }

        public EnemyDiedSignal(IDamagable enemy, Vector3 deathPosition)
        {
            Enemy = enemy;
            DeathPosition = deathPosition;
        }
    }
}