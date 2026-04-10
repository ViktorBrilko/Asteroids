using System;
using MVVM;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Binders
{
    public class ButtonBinder : IBinder
    {
        private readonly Button _view;
        private readonly UnityAction _modelAction;

        public ButtonBinder(Button view, Action model)
        {
            _view = view;
            _modelAction = new UnityAction(model);
        }

        public void Bind()
        {
            _view.onClick.AddListener(_modelAction);
        }

        public void Unbind()
        {
            _view.onClick.RemoveListener(_modelAction);
        }
    }
}
