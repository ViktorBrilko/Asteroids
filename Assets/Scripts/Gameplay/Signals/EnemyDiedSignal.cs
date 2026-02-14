namespace Gameplay.Signals
{
    public class EnemyDiedSignal
    {
        public IDamagable Enemy { get; }

        public EnemyDiedSignal(IDamagable enemy)
        {
            Enemy = enemy;
        }
    }
}