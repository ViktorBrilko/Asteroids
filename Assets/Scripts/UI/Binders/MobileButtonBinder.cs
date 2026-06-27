using System;
using Controls;
using MVVM;

namespace UI.Binders
{
    public class MobileButtonBinder : IBinder
    {
        private readonly Action<bool> _modelAction;
        private readonly MobileButton _view;

        public MobileButtonBinder(MobileButton view, Action<bool> model)
        {
            _view = view;
            _modelAction = model;
        }

        public void Bind()
        {
            _view.OnStateChanged += _modelAction;
        }

        public void Unbind()
        {
            _view.OnStateChanged -= _modelAction;
        }
    }
}