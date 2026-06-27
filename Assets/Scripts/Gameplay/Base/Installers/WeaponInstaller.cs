using Core.Configs;
using Gameplay.Players;
using Gameplay.Players.Weapons;
using UnityEngine;
using Zenject;

namespace Gameplay.Base.Installers
{
    public class WeaponInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _bulletsContainer;
        
        private ConfigProvider _provider;
        
        [Inject]
        public void Construct(ConfigProvider provider)
        {
            _provider = provider;
        }
        
        public override void InstallBindings()
        {
            InstallWeapons();
        }
        
        private void InstallWeapons()
        {
            Container.Bind<BulletConfig>().FromInstance(_provider.BulletConfig).AsSingle();
            Container.Bind<Core.IFactory<BulletProjectile>>().To<Core.Factory<BulletProjectile>>().AsSingle().WithArguments(_bulletPrefab);
            Container.Bind<ObjectPool<BulletProjectile>>().AsSingle()
                .WithArguments(_bulletsContainer.transform, _provider.CapacitiesConfig.BulletPoolCapacity)
                .OnInstantiated<ObjectPool<BulletProjectile>>((c, p) => p.Initialize());
            Container.BindInterfacesAndSelfTo<Spawner<BulletProjectile>>().AsSingle();

            Container.Bind<WeaponConfig>().FromInstance(_provider.WeaponConfig).AsSingle();
            Container.Bind<LaserWeapon>()
                .FromResolveGetter<Player>(playerInstance => playerInstance.GetComponent<LaserWeapon>())
                .AsSingle();
        }
    }
}