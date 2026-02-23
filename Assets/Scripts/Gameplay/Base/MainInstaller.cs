using Cinemachine;
using Core.Configs;
using Gameplay.Enemies.Asteroids;
using Gameplay.Gamefields;
using Gameplay.Players;
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
        [SerializeField] private GameObject _smallAsteroidPrefab;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _cameraPrefab;

        [SerializeField] private int _bulletPoolCapacity;
        [SerializeField] private int _asteroidPoolCapacity;
        [SerializeField] private int _smallAsteroidPoolCapacity;
        
        [SerializeField] private Transform _playerSpawnPoint;
        
        private ConfigProvider _provider;
    
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.DeclareSignal<ResetSignal<Bullet>>();
            Container.DeclareSignal<ResetSignal<Asteroid>>();
            Container.DeclareSignal<ResetSignal<SmallAsteroid>>();
            Container.DeclareSignal<EnemyDiedSignal>();
            Container.DeclareSignal<PlayerCollidedSignal>();
        
            _provider = new ConfigProvider();
            _provider.LoadAll();

            InstallEnemies();
            InstallBullets();
            InstallGameField();
            InstallCamera();
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
            GameObject asteroidsContainer = new("ASTEROIDS");
            
            Container.Bind<AsteroidConfig>().FromInstance(_provider.AsteroidConfig).AsSingle();
            Container.Bind<Core.IFactory<Asteroid>>().To<Core.Factory<Asteroid>>().AsSingle().WithArguments(_asteroidPrefab);
            Container.Bind<ObjectPool<Asteroid>>().AsSingle()
                .WithArguments(asteroidsContainer.transform, _asteroidPoolCapacity)
                .OnInstantiated<ObjectPool<Asteroid>>((c, p) => p.Initialize());
            Container.BindInterfacesAndSelfTo<Spawner<Asteroid>>().AsSingle();
            
            GameObject smallAsteroidsContainer = new("SMALL_ASTEROIDS");
            
            Container.Bind<SmallAsteroidConfig>().FromInstance(_provider.SmallAsteroidConfig).AsSingle();
            Container.Bind<Core.IFactory<SmallAsteroid>>().To<Core.Factory<SmallAsteroid>>().AsSingle().WithArguments(_smallAsteroidPrefab);
            Container.Bind<ObjectPool<SmallAsteroid>>().AsSingle()
                .WithArguments(smallAsteroidsContainer.transform, _smallAsteroidPoolCapacity)
                .OnInstantiated<ObjectPool<SmallAsteroid>>((c, p) => p.Initialize());
            Container.BindInterfacesAndSelfTo<Spawner<SmallAsteroid>>().AsSingle();
            
        }
        
        private void InstallCamera()
        {
            Container.Bind<CinemachineVirtualCamera>().FromComponentInNewPrefab(_cameraPrefab)
                .AsSingle().NonLazy();
        }


        private void InstallPlayer()
        {
            Container.Bind<PlayerConfig>().FromInstance(_provider.PlayerConfig).AsSingle(); 
            Container.Bind<Player>().FromComponentInNewPrefab(_playerPrefab).UnderTransform(_playerSpawnPoint).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerInputHandler>().AsSingle().NonLazy();
        }
        
        
        private void InstallGameField()
        {
            Container.Bind<GameFieldConfig>().FromInstance(_provider.GameFieldConfig).AsSingle();
            Container.Bind<GameField>().FromComponentInNewPrefab(_gameFieldPrefab).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<EnemyGeneratorService>().AsSingle();
        }
    }
}