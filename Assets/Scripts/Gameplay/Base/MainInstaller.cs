using Cinemachine;
using Core.Configs;
using Gameplay.Enemies;
using Gameplay.Gamefields;
using Gameplay.Players;
using Gameplay.Scores;
using Gameplay.Signals;
using Gameplay.Weapons;
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
        [SerializeField] private GameObject _ufoPrefab;

        [SerializeField] private int _bulletPoolCapacity;
        [SerializeField] private int _asteroidPoolCapacity;
        [SerializeField] private int _smallAsteroidPoolCapacity;
        [SerializeField] private int _ufoPoolCapacity;

        [SerializeField] private Transform _playerSpawnPoint;

        private ConfigProvider _provider;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<ResetSignal<Bullet>>();
            Container.DeclareSignal<ResetSignal<Asteroid>>();
            Container.DeclareSignal<ResetSignal<SmallAsteroid>>();
            Container.DeclareSignal<ResetSignal<Ufo>>();
            Container.DeclareSignal<EnemyDiedSignal>();
            Container.DeclareSignal<PlayerCollidedSignal>();

            _provider = new ConfigProvider();
            _provider.LoadAll();

            InstallScore();
            InstallEnemies();
            InstallPlayer();
            InstallWeapons();
            InstallGameField();
            InstallCamera();
            
        }

        private void InstallScore()
        {
            Container.Bind<ScoreConfig>().FromInstance(_provider.ScoreConfig).AsSingle();
            //TODO убрать NonLazy
            Container.BindInterfacesAndSelfTo<ScoreLogic>().AsSingle().NonLazy();
        }

        private void InstallWeapons()
        {
            GameObject bulletContainer = new("BULLETS");
            Container.Bind<BulletConfig>().FromInstance(_provider.BulletConfig).AsSingle();
            Container.Bind<Core.IFactory<Bullet>>().To<Core.Factory<Bullet>>().AsSingle().WithArguments(_bulletPrefab);
            Container.Bind<ObjectPool<Bullet>>().AsSingle()
                .WithArguments(bulletContainer.transform, _bulletPoolCapacity)
                .OnInstantiated<ObjectPool<Bullet>>((c, p) => p.Initialize());
            Container.BindInterfacesAndSelfTo<Spawner<Bullet>>().AsSingle();
            
            Container.Bind<WeaponConfig>().FromInstance(_provider.WeaponConfig).AsSingle();
            Container.Bind<PlayerWeapon>()
                .FromResolveGetter<Player>(playerInstance => playerInstance.GetComponent<PlayerWeapon>())
                .AsSingle();
        }

        private void InstallEnemies()
        {
            GameObject asteroidsContainer = new("ASTEROIDS");

            Container.Bind<AsteroidConfig>().FromInstance(_provider.AsteroidConfig).AsSingle();
            Container.Bind<Core.IFactory<Asteroid>>().To<Core.Factory<Asteroid>>().AsSingle()
                .WithArguments(_asteroidPrefab);
            Container.Bind<ObjectPool<Asteroid>>().AsSingle()
                .WithArguments(asteroidsContainer.transform, _asteroidPoolCapacity)
                .OnInstantiated<ObjectPool<Asteroid>>((c, p) => p.Initialize());
            Container.BindInterfacesAndSelfTo<Spawner<Asteroid>>().AsSingle();
            Container.Resolve<ScoreLogic>().EnemyScoreRates
                .Add(EnemyTypes.Asteroid, _provider.ScoreConfig.ScoreForAsteroid);

            GameObject smallAsteroidsContainer = new("SMALL_ASTEROIDS");

            Container.Bind<SmallAsteroidConfig>().FromInstance(_provider.SmallAsteroidConfig).AsSingle();
            Container.Bind<Core.IFactory<SmallAsteroid>>().To<Core.Factory<SmallAsteroid>>().AsSingle()
                .WithArguments(_smallAsteroidPrefab);
            Container.Bind<ObjectPool<SmallAsteroid>>().AsSingle()
                .WithArguments(smallAsteroidsContainer.transform, _smallAsteroidPoolCapacity)
                .OnInstantiated<ObjectPool<SmallAsteroid>>((c, p) => p.Initialize());
            Container.BindInterfacesAndSelfTo<Spawner<SmallAsteroid>>().AsSingle();
            Container.Resolve<ScoreLogic>().EnemyScoreRates
                .Add(EnemyTypes.SmallAsteroid, _provider.ScoreConfig.ScoreForSmallAsteroid);

            GameObject ufoContainer = new("UFO");

            Container.Bind<UfoConfig>().FromInstance(_provider.UfoConfig).AsSingle();
            Container.Bind<Core.IFactory<Ufo>>().To<Core.Factory<Ufo>>().AsSingle().WithArguments(_ufoPrefab);
            Container.Bind<ObjectPool<Ufo>>().AsSingle()
                .WithArguments(ufoContainer.transform, _ufoPoolCapacity)
                .OnInstantiated<ObjectPool<Ufo>>((c, p) => p.Initialize());
            Container.BindInterfacesAndSelfTo<Spawner<Ufo>>().AsSingle();
            Container.Resolve<ScoreLogic>().EnemyScoreRates
                .Add(EnemyTypes.Ufo, _provider.ScoreConfig.ScoreForUfo);
        }

        private void InstallCamera()
        {
            Container.Bind<CinemachineVirtualCamera>().FromComponentInNewPrefab(_cameraPrefab)
                .AsSingle().NonLazy();
        }


        private void InstallPlayer()
        {
            Container.Bind<PlayerConfig>().FromInstance(_provider.PlayerConfig).AsSingle();
            Container.Bind<Player>().FromComponentInNewPrefab(_playerPrefab).UnderTransform(_playerSpawnPoint)
                .AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerInputHandler>().AsSingle().NonLazy();
            
            Container.Bind<PlayerMovement>()
                .FromResolveGetter<Player>(playerInstance => playerInstance.GetComponent<PlayerMovement>())
                .AsSingle();
            
            Container.Bind<HealthService>()
                .FromResolveGetter<Player>(playerInstance => playerInstance.GetComponent<HealthService>())
                .AsSingle();
        }


        private void InstallGameField()
        {
            Container.Bind<GameFieldConfig>().FromInstance(_provider.GameFieldConfig).AsSingle();
            Container.Bind<GameField>().FromComponentInNewPrefab(_gameFieldPrefab).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<EnemyGeneratorService>().AsSingle();
        }
    }
}