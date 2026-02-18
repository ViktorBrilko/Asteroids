using Core.Configs;
using Gameplay.Enemies.Asteroids;
using Gameplay.Gamefields;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Base
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private GameObject _gameFieldPrefab;
        [SerializeField] private GameObject _asteroidPrefab;

        [SerializeField] private int _bulletPoolCapacity;
        [SerializeField] private int _asteroidPoolCapacity;
        
        private ConfigProvider _provider;
    
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.DeclareSignal<ResetSignal<Bullet>>();
            Container.DeclareSignal<ResetSignal<Asteroid>>();
            Container.DeclareSignal<EnemyDiedSignal>();
            Container.DeclareSignal<PlayerCollidedSignal>();
        
            _provider = new ConfigProvider();
            _provider.LoadAll();

            InstallEnemies();
            InstallBullets();
            InstallGameField();
            InstallPlayer();
           

        }

        private void InstallBullets()
        {
            GameObject bulletContainer = new("BULLETS");
            Container.Bind<BulletConfig>().FromInstance(_provider.BulletConfig).AsSingle();
            Container.Bind<Core.IFactory<Bullet>>().To<Core.Factory<Bullet>>().AsSingle().WithArguments(_bulletPrefab);
            Container.Bind<ObjectPool<Bullet>>().AsSingle()
                .WithArguments(bulletContainer.transform, _bulletPoolCapacity)
                .OnInstantiated<ObjectPool<Bullet>>((c, p) => p.Initialize());
            Container.BindInterfacesAndSelfTo<Spawner<Bullet>>().AsSingle();
        }

        private void InstallEnemies()
        {
            GameObject enemyContainer = new("ENEMIES");
            
            Container.Bind<AsteroidConfig>().FromInstance(_provider.AsteroidConfig).AsSingle();
            Container.Bind<Core.IFactory<Asteroid>>().To<Core.Factory<Asteroid>>().AsSingle().WithArguments(_asteroidPrefab);
            Container.Bind<ObjectPool<Asteroid>>().AsSingle()
                .WithArguments(enemyContainer.transform, _asteroidPoolCapacity)
                .OnInstantiated<ObjectPool<Asteroid>>((c, p) => p.Initialize());
            Container.BindInterfacesAndSelfTo<Spawner<Asteroid>>().AsSingle();
        }

        private void InstallPlayer()
        {
            Container.Bind<PlayerConfig>().FromInstance(_provider.PlayerConfig).AsSingle(); 
        }
        
        private void InstallGameField()
        {
            Container.Bind<GameFieldConfig>().FromInstance(_provider.GameFieldConfig).AsSingle();
            Container.Bind<GameField>().FromComponentInNewPrefab(_gameFieldPrefab).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<EnemyGeneratorService>().AsSingle();
        }
    }
}