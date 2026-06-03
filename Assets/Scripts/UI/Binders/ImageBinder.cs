using System;
using MVVM;
using UniRx;
using UnityEngine.UI;

namespace UI.Binders
{
    public class ImageBinder : IBinder, IObserver<float>
    {
        private IDisposable _handle;
        private readonly ReactiveProperty<float> _property;
        private readonly Image _view;

        public ImageBinder(Image view, ReactiveProperty<float> property)
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
            _view.fillAmount = value;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }
    }
}