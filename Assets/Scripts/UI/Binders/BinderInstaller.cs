using MVVM;
using Zenject;

namespace UI.Binders
{
    public class BinderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BinderFactory.RegisterBinder<TextBinder>();
            BinderFactory.RegisterBinder<ImageBinder>();
            BinderFactory.RegisterBinder<ListImageBinder>();
            BinderFactory.RegisterBinder<ButtonBinder>();
            BinderFactory.RegisterBinder<MobileButtonBinder>();
            BinderFactory.RegisterBinder<SliderBinder>();
            BinderFactory.RegisterBinder<ViewSetterBinder<bool>>();
            BinderFactory.RegisterBinder<NumberBinder>();
        }
    }
}