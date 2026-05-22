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
        private enum BindingMode
        {
            FromInstance = 0,
            FromResolve = 1
        }

        [SerializeField] private BindingMode viewBinding;

        [SerializeField] private Object view;

        [Space(8)] [SerializeField] private BindingMode viewModelBinding;

        [SerializeField, HideInInspector] private string viewModelTypeName;

        [Inject] private DiContainer diContainer;

        private IBinder _binder;
        private Type _viewModelType;

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

#if UNITY_EDITOR
        [SerializeField] private MonoScript _viewModelScript;

        private void OnValidate()
        {
            if (_viewModelScript != null)
            {
                viewModelTypeName = _viewModelScript.GetClass()?.AssemblyQualifiedName;
            }
            else
            {
                viewModelTypeName = string.Empty;
            }
        }
#endif

        private IBinder CreateBinder()
        {
            object view = this.viewBinding switch
            {
                BindingMode.FromInstance => this.view,
                _ => throw new Exception($"Binding type of view {this.viewBinding} is not found!")
            };

            object model = this.viewModelBinding switch
            {
                BindingMode.FromResolve => this.diContainer.Resolve(_viewModelType),
                _ => throw new Exception($"Binding type of view {this.viewBinding} is not found!")
            };

            return BinderFactory.CreateComposite(view, model);
        }
    }
}