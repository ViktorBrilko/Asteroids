using Cinemachine;
using Controls;
using Core.Configs;
using Gameplay.Players;
using UnityEngine;
using Zenject;

namespace Gameplay.Base.Installers
{
    public class PlayerInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _cameraPrefab;
        [SerializeField] private GameObject _mobileButtonsPrefab;
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Canvas _canvas;

        private bool _isMobile;
        private ConfigProvider _provider;

        [Inject]
        public void Construct(ConfigProvider provider)
        {
            _provider = provider;
        }
        
        public void Initialize()
        {
            if (_isMobile) Container.InstantiatePrefab(_mobileButtonsPrefab, _canvas.transform);

            _playerSpawnPoint.DetachChildren();
        }

        public override void InstallBindings()
        {
            _isMobile = Application.isMobilePlatform;

            InstallControls();
            InstallCamera();
            InstallPlayer();

            Container.BindInterfacesTo<PlayerInstaller>().FromInstance(this);
        }

        private void InstallControls()
        {
            if (_isMobile)
                Container.Bind<MobileController>().AsSingle();
            else
                Container.BindInterfacesAndSelfTo<DesktopController>().AsSingle();
        }

        private void InstallCamera()
        {
            Container.Bind<Camera>().FromInstance(Camera.main).AsSingle();
            Container.Bind<CinemachineVirtualCamera>().FromComponentInNewPrefab(_cameraPrefab).AsSingle().NonLazy();
        }

        private void InstallPlayer()
        {
            Container.Bind<PlayerConfig>().FromInstance(_provider.PlayerConfig).AsSingle();
            Container.Bind<Player>().FromComponentInNewPrefab(_playerPrefab).UnderTransform(_playerSpawnPoint)
                .AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerActionCommands>().AsSingle().NonLazy();

            Container.Bind<PlayerMovement>()
                .FromResolveGetter<Player>(playerInstance => playerInstance.GetComponent<PlayerMovement>())
                .AsSingle();
            
            Container.Bind<PlayerInertia>()
                .FromResolveGetter<Player>(playerInstance => playerInstance.GetComponent<PlayerInertia>())
                .AsSingle();

            Container.Bind<HealthComponent>()
                .FromResolveGetter<Player>(playerInstance => playerInstance.GetComponent<HealthComponent>())
                .AsSingle();
        }
    }
}