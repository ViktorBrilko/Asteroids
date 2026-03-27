using System;
using System.Collections.Generic;
using MVVM;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Binders
{
    public class ListImageBinder : IBinder, IObserver<int>
    {
        private List<Image> _view;
        private ReactiveProperty<int> _property;
        private IDisposable _handle;

        public ListImageBinder(List<Image> view, ReactiveProperty<int> property)
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

        public void OnNext(int value)
        {
            for (int i = 0; i < _view.Count; i++)
            {
                if (i < value)
                {
                    _view[i].fillAmount = 1;
                }
                else
                {
                    _view[i].fillAmount = 0;
                }
            }
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }
    }
}