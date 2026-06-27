using Core.Configs;
using Gameplay.Signals;
using Zenject;

namespace Gameplay.Enemies
{
    public class SmallAsteroid : Enemy
    {
        private SmallAsteroidConfig _config;

        public override BaseEnemyConfig Config =>  _config;

        [Inject]
        public void Construct( SmallAsteroidConfig config)
        {
            _config = config;
            EnemyType = EnemyType.SmallAsteroid;
        }

        protected override void FireResetSignal()
        {
            SignalBus.Fire(new ResetSignal<SmallAsteroid>(this));
        }
    }
}