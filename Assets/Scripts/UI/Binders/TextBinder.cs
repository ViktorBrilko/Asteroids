using System;
using MVVM;
using TMPro;
using UniRx;

namespace UI.Binders
{
    public class TextBinder : IBinder, IObserver<string>
    {
        private readonly ReactiveProperty<string> _property;
        private readonly TMP_Text _view;
        private IDisposable _handle;

        public TextBinder(TMP_Text view, ReactiveProperty<string> property)
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

        public void OnNext(string value)
        {
            _view.text = value;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }
    }
}