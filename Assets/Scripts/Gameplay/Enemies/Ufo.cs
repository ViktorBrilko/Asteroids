using Core.Configs;
using Gameplay.Signals;
using Zenject;

namespace Gameplay.Enemies
{
    public class Ufo : Enemy
    {
        private UfoConfig _config;

        public override BaseEnemyConfig Config => _config;

        [Inject]
        public void Construct(UfoConfig config)
        {
            _config = config;
            EnemyType = EnemyType.Ufo;
        }

        protected override void FireResetSignal()
        {
            SignalBus.Fire(new ResetSignal<Ufo>(this));
        }
    }
}