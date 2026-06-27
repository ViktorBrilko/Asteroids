using Core.Configs;
using Gameplay.Enemies;
using UnityEngine;
using Zenject;

namespace Gameplay.Base.Installers
{
    public class EnemyInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _asteroidPrefab;
        [SerializeField] private GameObject _smallAsteroidPrefab;
        [SerializeField] private GameObject _ufoPrefab;
        [SerializeField] private Transform _asteroidsContainer;
        [SerializeField] private Transform _smallAsteroidsContainer;
        [SerializeField] private Transform _ufosContainer;

        private ConfigProvider _provider;
        
        [Inject]
        public void Construct(ConfigProvider provider)
        {
            _provider = provider;
        }
        
        public override void InstallBindings()
        {
            InstallEnemies();
        }
        
        private void InstallEnemies()
        {
            Container.Bind<AsteroidConfig>().FromInstance(_provider.AsteroidConfig).AsSingle();
            Container.Bind<Core.IFactory<Asteroid>>().To<Core.Factory<Asteroid>>().AsSingle()
                .WithArguments(_asteroidPrefab);
            Container.Bind<ObjectPool<Asteroid>>().AsSingle()
                .WithArguments(_asteroidsContainer.transform, _provider.CapacitiesConfig.AsteroidPoolCapacity)
                .OnInstantiated<ObjectPool<Asteroid>>((c, p) => p.Initialize());
            Container.BindInterfacesAndSelfTo<Spawner<Asteroid>>().AsSingle();

            Container.Bind<SmallAsteroidConfig>().FromInstance(_provider.SmallAsteroidConfig).AsSingle();
            Container.Bind<Core.IFactory<SmallAsteroid>>().To<Core.Factory<SmallAsteroid>>().AsSingle()
                .WithArguments(_smallAsteroidPrefab);
            Container.Bind<ObjectPool<SmallAsteroid>>().AsSingle()
                .WithArguments(_smallAsteroidsContainer.transform,  _provider.CapacitiesConfig.SmallAsteroidPoolCapacity)
                .OnInstantiated<ObjectPool<SmallAsteroid>>((c, p) => p.Initialize());
            Container.BindInterfacesAndSelfTo<Spawner<SmallAsteroid>>().AsSingle();

            Container.Bind<UfoConfig>().FromInstance(_provider.UfoConfig).AsSingle();
            Container.Bind<Core.IFactory<Ufo>>().To<Core.Factory<Ufo>>().AsSingle().WithArguments(_ufoPrefab);
            Container.Bind<ObjectPool<Ufo>>().AsSingle()
                .WithArguments(_ufosContainer.transform,  _provider.CapacitiesConfig.UfoPoolCapacity)
                .OnInstantiated<ObjectPool<Ufo>>((c, p) => p.Initialize());
            Container.BindInterfacesAndSelfTo<Spawner<Ufo>>().AsSingle();
        }
    }
}