using System;
using MVVM;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Binders
{
    public class ImageBinder : IBinder, IObserver<float>
    {
        private Image _view;
        private ReactiveProperty<float> _property;
        private IDisposable _handle;

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