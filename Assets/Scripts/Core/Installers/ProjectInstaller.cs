using Analytics;
using Analytics.Firebase;
using Core.Audios;
using Core.Configs;
using Core.Signals;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Core.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _audioServicePrefab;
        [SerializeField] private AudioMixer _audioMixer;

        private ConfigProvider _provider;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<PlayerDiedSignal>();
            Container.DeclareSignal<AdClosedSignal>();
            Container.DeclareSignal<PanelChangeStateSignal>();
            Container.DeclareSignal<PauseGameSignal>();

            InstallConfigProvider();
            InstallSettings();
            InstallWindowsState();
            InstallLoadLevelService();
            InstallAudio();
            InstallAnalytics();
            InstallPause();
        }

        private void InstallAnalytics()
        {
            Container.Bind<IAnalyticsService>().To<FirebaseAnalyticsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<FirebaseInitializer>().AsSingle();
        }

        private void InstallAudio()
        {
            Container.Bind<AudioMixer>().FromInstance(_audioMixer).AsSingle();
            Container.BindInterfacesAndSelfTo<AudioService>().FromComponentInNewPrefab(_audioServicePrefab).AsSingle()
                .NonLazy();
        }

        private void InstallLoadLevelService()
        {
            Container.BindInterfacesAndSelfTo<LoadLevelService>().AsSingle().NonLazy();
        }

        private void InstallWindowsState()
        {
            Container.Bind<WindowsState>().AsSingle();
        }

        private void InstallSettings()
        {
            Container.Bind<Settings>().AsSingle();
            Container.Bind<SettingsConfig>().FromInstance(_provider.SettingsConfig).AsSingle();
        }

        private void InstallConfigProvider()
        {
            _provider = new ConfigProvider();
            _provider.LoadAll();
            Container.Bind<ConfigProvider>().FromInstance(_provider).AsSingle();
        }
        
        private void InstallPause()
        {
            Container.BindInterfacesAndSelfTo<PauseService>().AsSingle().NonLazy();
        }
    }
}