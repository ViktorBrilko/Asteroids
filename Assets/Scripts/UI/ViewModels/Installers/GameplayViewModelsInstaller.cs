using UnityEngine;
using Zenject;

namespace UI.ViewModels.Installers
{
    public class GameplayViewModelsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ScoreViewModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LaserViewModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerParametersViewModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InertiaViewModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<HealthViewModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DeathPanelViewModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameMenuViewModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameMenuButtonViewModel>().AsSingle().NonLazy();

            if (Application.isMobilePlatform)
                Container.BindInterfacesAndSelfTo<MobileButtonsViewModel>().AsSingle().NonLazy();
        }
    }
}