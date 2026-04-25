using System;
using MVVM;
using UniRx;

namespace UI.Binders
{
    public class NumberBinder : IBinder, IObserver<float>
    {
        private Action<float> _view;
        private ReactiveProperty<float> _property;
        private IDisposable _handle;

        public NumberBinder(Action<float> view, ReactiveProperty<float> property)
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

        public void OnNext(float value)
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