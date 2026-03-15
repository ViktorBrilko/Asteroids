using System;
using MVVM;
using UnityEditor;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public sealed class MonoViewBinder : MonoBehaviour
{
    private enum BindingMode
    {
        FromInstance = 0,
        FromResolve = 1,
        FromResolveId = 2
    }

    [SerializeField] private BindingMode viewBinding;

    [SerializeField] private Object view;

    [SerializeField] private MonoScript viewType;

    [SerializeField] private string viewId;

    [Space(8)] [SerializeField] private BindingMode viewModelBinding;

    [SerializeField] private Object viewModel;

    [SerializeField] private MonoScript viewModelType;

    [SerializeField] private string viewModelId;

    [Inject] private DiContainer diContainer;

    private IBinder _binder;

    private void Awake()
    {
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
        object view = this.viewBinding switch
        {
            BindingMode.FromInstance => this.view,
            BindingMode.FromResolve => this.diContainer.Resolve(this.viewType.GetClass()),
            BindingMode.FromResolveId => this.diContainer.ResolveId(this.viewType.GetClass(), this.viewId),
            _ => throw new Exception($"Binding type of view {this.viewBinding} is not found!")
        };

        object model = this.viewModelBinding switch
        {
            BindingMode.FromInstance => this.viewModel,
            BindingMode.FromResolve => this.diContainer.Resolve(this.viewModelType.GetClass()),
            BindingMode.FromResolveId => this.diContainer.ResolveId(this.viewModelType.GetClass(), this.viewModelId),
            _ => throw new Exception($"Binding type of view {this.viewBinding} is not found!")
        };

        return BinderFactory.CreateComposite(view, model);
    }
}