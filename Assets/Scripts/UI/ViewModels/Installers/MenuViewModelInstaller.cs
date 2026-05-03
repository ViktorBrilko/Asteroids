using Zenject;

namespace UI.ViewModels
{
    public class MenuViewModelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MainMenuViewModel>().AsSingle().NonLazy();
        }
    }
}