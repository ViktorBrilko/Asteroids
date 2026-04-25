using Core.Audios;
using UnityEngine;
using Zenject;

namespace UI.ViewModels
{
    public class MenuViewModelInstaller : MonoInstaller
    {
        [SerializeField] private Settings _settings;

        public override void InstallBindings()
        {
            Container.Bind<Settings>().FromInstance(_settings).AsSingle();
            Container.Bind<WindowsState>().AsSingle();

            Container.BindInterfacesAndSelfTo<MenuPanelViewModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SettingsViewModel>().AsSingle().NonLazy();
        }
    }
}