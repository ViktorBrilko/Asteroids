using Analytics;
using Analytics.Firebase;
using Core.Audios;
using Core.Configs;
using Core.Signals;
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
            SignalBusInstaller.Install(Container);
            
            _provider = new ConfigProvider();
            _provider.LoadAll();
            Container.Bind<ConfigProvider>().FromInstance(_provider).AsSingle();

            Container.DeclareSignal<PlayerDiedSignal>();
            
            Container.Bind<Settings>().AsSingle();
            Container.Bind<SettingsConfig>().FromInstance(_provider.SettingsConfig).AsSingle();

            Container.Bind<WindowsState>().AsSingle();

            Container.BindInterfacesAndSelfTo<LoadLevelService>().AsSingle().NonLazy();

            Container.Bind<AudioMixer>().FromInstance(_audioMixer).AsSingle();
            Container.BindInterfacesAndSelfTo<AudioService>().FromComponentInNewPrefab(_audioServicePrefab).AsSingle()
                .NonLazy();
            
            Container.Bind<IAnalyticsService>().To<FirebaseAnalyticsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<FirebaseInitializer>().AsSingle();
        }
    }
}