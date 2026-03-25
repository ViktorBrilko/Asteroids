using Zenject;

namespace UI.ViewModels
{
    public class ViewModelsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ScoreViewModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LaserCountViewModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LaserChargeViewModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerRotationViewModel>().AsSingle().NonLazy();
          Container.BindInterfacesAndSelfTo<PlayerPositionViewModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SpeedViewModel>().AsSingle().NonLazy();
        }
    }
}