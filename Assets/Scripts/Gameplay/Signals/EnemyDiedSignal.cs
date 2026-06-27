using Gameplay.Base;
using UnityEngine;

namespace Gameplay.Signals
{
    public class EnemyDiedSignal
    {
        public IKillable Enemy { get; }
        public Vector3 DeathPosition { get; }
        
        public EnemyDiedSignal(IKillable enemy, Vector3 deathPosition)
        {
            Enemy = enemy;
            DeathPosition = deathPosition;
        }
    }
}