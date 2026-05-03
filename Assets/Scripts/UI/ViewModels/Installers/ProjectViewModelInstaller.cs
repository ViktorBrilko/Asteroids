using Zenject;

namespace UI.ViewModels.Installers
{
    public class ProjectViewModelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SettingsViewModel>().AsSingle().NonLazy();
        }
    }
}