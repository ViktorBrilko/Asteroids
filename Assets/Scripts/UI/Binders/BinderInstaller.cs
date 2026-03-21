using MVVM;
using UnityEngine;
using Zenject;

namespace UI.Binders
{
    public class BinderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BinderFactory.RegisterBinder<TextBinder>();
            BinderFactory.RegisterBinder<ImageBinder>();
        }
    }
}