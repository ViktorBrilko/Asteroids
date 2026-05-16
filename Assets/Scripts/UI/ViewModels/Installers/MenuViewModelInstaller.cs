using Zenject;

namespace UI.ViewModels.Installers
{
    public class MenuViewModelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MainMenuViewModel>().AsSingle().NonLazy();
        }
    }
}