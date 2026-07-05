using Gameplay.Enemies;
using Zenject;

namespace Gameplay.Base.Installers
{
    public class ConcreteEnemyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<HealthComponent>()
                .FromComponentInHierarchy()
                .AsSingle();
            
            Container.Bind<EnemyMovement>()
                .FromComponentInHierarchy()
                .AsSingle();
        }
    }
}