using Core.Configs;
using Zenject;

namespace Ads
{
    public class AdsInstaller : MonoInstaller
    {
        private ConfigProvider _provider;

        [Inject]
        public void Construct(ConfigProvider provider)
        {
            _provider = provider;
        }

        public override void InstallBindings()
        {
          Container.Bind<AdsConfig>().FromInstance(_provider.AdsConfig).AsSingle();
          Container.BindInterfacesAndSelfTo<AdsManager>().AsSingle().NonLazy();
          Container.BindExecutionOrder<AdsManager>(-10);
        }
    }
}