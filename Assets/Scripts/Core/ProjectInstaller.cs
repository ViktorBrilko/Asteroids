using Core.Audios;
using Core.Configs;
using UnityEngine;
using Zenject;

namespace Core
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _audioServicePrefab;
        
        private ConfigProvider _provider;
        
        public override void InstallBindings()
        {
            _provider = new ConfigProvider();
            _provider.LoadAll();
            Container.Bind<ConfigProvider>().FromInstance(_provider).AsSingle();
            
            Container.Bind<SettingsConfig>().FromInstance(_provider.SettingsConfig).AsSingle();
            
            Container.Bind<LoadLevelService>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<AudioService>().FromComponentInNewPrefab(_audioServicePrefab).AsSingle().NonLazy();
        }
    }
}