using Gameplay.Base;
using UnityEngine;

namespace Gameplay.Signals
{
    public class EnemyDiedSignal
    {
        public EnemyDiedSignal(IDieable enemy, Vector3 deathPosition)
        {
            Enemy = enemy;
            DeathPosition = deathPosition;
        }

        public IDieable Enemy { get; }
        public Vector3 DeathPosition { get; }
    }
}