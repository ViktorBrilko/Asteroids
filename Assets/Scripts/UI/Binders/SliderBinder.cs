using System;
using MVVM;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Binders
{
    public class SliderBinder : IBinder
    {
        private readonly Slider _view;
        private readonly UnityAction<float> _modelAction;

        public SliderBinder(Slider view, Action<float> model)
        {
            _view = view;
            _modelAction = new UnityAction<float>(model);
        }

        public void Bind()
        {
            _view.onValueChanged.AddListener(_modelAction);
        }

        public void Unbind()
        {
            _view.onValueChanged.RemoveListener(_modelAction);
        }
    }
}