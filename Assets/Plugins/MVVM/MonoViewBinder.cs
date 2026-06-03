using System;
using MVVM;
using UnityEditor;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Plugins.MVVM
{
    public sealed class MonoViewBinder : MonoBehaviour
    {
        [SerializeField] private BindingMode viewBinding;

        [SerializeField] private Object view;

        [Space(8)] [SerializeField] private BindingMode viewModelBinding;

        [SerializeField] [HideInInspector] private string viewModelTypeName;

        private IBinder _binder;
        private Type _viewModelType;

        [Inject] private DiContainer diContainer;

        private void Awake()
        {
            _viewModelType = Type.GetType(viewModelTypeName);
            _binder = CreateBinder();
        }

        private void OnEnable()
        {
            _binder.Bind();
        }

        private void OnDisable()
        {
            _binder.Unbind();
        }

        private IBinder CreateBinder()
        {
            object view = viewBinding switch
            {
                BindingMode.FromInstance => this.view,
                _ => throw new Exception($"Binding type of view {viewBinding} is not found!")
            };

            var model = viewModelBinding switch
            {
                BindingMode.FromResolve => diContainer.Resolve(_viewModelType),
                _ => throw new Exception($"Binding type of view {viewBinding} is not found!")
            };

            return BinderFactory.CreateComposite(view, model);
        }

        private enum BindingMode
        {
            FromInstance = 0,
            FromResolve = 1
        }

#if UNITY_EDITOR
        [SerializeField] private MonoScript _viewModelScript;

        private void OnValidate()
        {
            if (_viewModelScript != null)
                viewModelTypeName = _viewModelScript.GetClass()?.AssemblyQualifiedName;
            else
                viewModelTypeName = string.Empty;
        }
#endif
    }
}