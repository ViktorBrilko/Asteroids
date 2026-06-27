using Core.Configs;
using Gameplay.Signals;
using Zenject;

namespace Gameplay.Enemies
{
    public class Asteroid : Enemy
    {
        private AsteroidConfig _config;

        public override BaseEnemyConfig Config => _config;

        [Inject]
        public void Construct(AsteroidConfig config)
        {
            _config = config;
            EnemyType = EnemyType.Asteroid;
        }

        protected override void FireResetSignal()
        {
            SignalBus.Fire(new ResetSignal<Asteroid>(this));
        }
    }
}