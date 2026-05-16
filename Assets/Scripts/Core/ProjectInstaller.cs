using Core.Audios;
using Core.Configs;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Core
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _audioServicePrefab;
        [SerializeField] private AudioMixer _audioMixer;

        private ConfigProvider _provider;

        public override void InstallBindings()
        {
            _provider = new ConfigProvider();
            _provider.LoadAll();
            Container.Bind<ConfigProvider>().FromInstance(_provider).AsSingle();
            
            Container.Bind<Settings>().AsSingle();
            Container.Bind<SettingsConfig>().FromInstance(_provider.SettingsConfig).AsSingle();

            Container.Bind<WindowsState>().AsSingle();

            Container.Bind<LoadLevelService>().AsSingle().NonLazy();

            Container.Bind<AudioMixer>().FromInstance(_audioMixer).AsSingle();
            Container.BindInterfacesAndSelfTo<AudioService>().FromComponentInNewPrefab(_audioServicePrefab).AsSingle()
                .NonLazy();
            
            
        }
    }
}