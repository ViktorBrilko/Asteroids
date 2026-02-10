using Core.Configs;
using Gameplay.Gamefields;
using UnityEngine;
using Zenject;

namespace Gameplay.Base
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private GameObject _gameFieldPrefab;

        [SerializeField] private int _bulletPoolCapacity;
        
        private ConfigProvider _provider;
    
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.DeclareSignal<ResetSignal<Bullet>>();
        
            _provider = new ConfigProvider();
            _provider.LoadAll();

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

        private void InstallPlayer()
        {
            Container.Bind<PlayerConfig>().FromInstance(_provider.PlayerConfig).AsSingle(); 
        }
        
        private void InstallGameField()
        {
            Container.Bind<GameFieldConfig>().FromInstance(_provider.GameField).AsSingle();
            Container.Bind<GameField>().FromComponentInNewPrefab(_gameFieldPrefab).AsSingle().NonLazy();
        }
    }
}