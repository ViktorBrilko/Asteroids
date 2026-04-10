using System;
using MVVM;
using UniRx;

namespace UI.Binders
{
    public class ViewSetterBinder<T> : IBinder, IObserver<T>
    {
        private readonly Action<T> _view;
        private readonly IReadOnlyReactiveProperty<T> _property;
        private IDisposable _handle;

        public ViewSetterBinder(Action<T> view, IReadOnlyReactiveProperty<T> property)
        {
            _view = view;
            _property = property;
        }

        public void Bind()
        {
            OnNext(_property.Value);
            _handle = _property.Subscribe(this);
        }

        public void Unbind()
        {
            _handle?.Dispose();
            _handle = null;
        }

        public void OnNext(T value)
        {
            _view.Invoke(value);
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }
    }
}