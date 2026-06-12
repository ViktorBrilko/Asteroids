using System;
using Controls;
using Core.Signals;
using MVVM;
using UniRx;
using Zenject;

namespace UI.ViewModels
{
    public class MobileButtonsViewModel : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        [Data("MobileButtonsPanelState")] public readonly ReactiveProperty<bool> IsPanelActive = new(true);
        public readonly MobileController MobileController;

        public MobileButtonsViewModel(MobileController mobileController, SignalBus signalBus)
        {
            MobileController = mobileController;
            _signalBus = signalBus;
        }

        [Setter("RotatingLeft")]
        public bool RotateLeft
        {
            set => MobileController.RotateLeft(value);
        }

        [Setter("RotatingRight")]
        public bool RotateRight
        {
            set => MobileController.RotateRight(value);
        }

        [Setter("MovingState")]
        public bool ChangeMovingState
        {
            set => MobileController.ChangingMovingState(value);
        }

        [Setter("YDirection")]
        public float YDirection
        {
            set => MobileController.SetYDirection(value);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PanelChangeStateSignal>(OnPanelChangeStateSignal);
        }

        public void Initialize()
        {
            _signalBus.Subscribe<PanelChangeStateSignal>(OnPanelChangeStateSignal);
        }

        private void OnPanelChangeStateSignal(PanelChangeStateSignal signal)
        {
            if (signal.State)
                IsPanelActive.Value = false;
            else
                IsPanelActive.Value = true;
        }

        [Method("FireLaser")]
        public void FireLaser()
        {
            MobileController.FireLaser();
        }

        [Method("FireBullets")]
        public void FireBullets()
        {
            MobileController.FireBullets();
        }
    }
}