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
        [Data("MobileButtonsPanelState")] public readonly ReactiveProperty<bool> IsPanelActive = new(true);
        
        private readonly MobileController _mobileController;
        private readonly SignalBus _signalBus;

        public void Dispose()
        {
            _signalBus.Unsubscribe<PanelChangeStateSignal>(OnPanelChangeStateSignal);
        }

        public void Initialize()
        {
            _signalBus.Subscribe<PanelChangeStateSignal>(OnPanelChangeStateSignal);
        }

        public MobileButtonsViewModel(MobileController mobileController, SignalBus signalBus)
        {
            _mobileController = mobileController;
            _signalBus = signalBus;
        }

        [Method("RotatingLeft")]
        public void OnLeftRotation(bool isRotating)
        {
            _mobileController.RotateLeft(isRotating);
        }

        [Method("RotatingRight")]
        public void OnRightRotation(bool isRotating)
        {
            _mobileController.RotateRight(isRotating);
        }

        [Method("MoveForward")]
        public void OnForwardMoving(bool isMoving)
        {
            if (isMoving)
                _mobileController.SetYDirection(1);
            else
                _mobileController.SetYDirection(0);

            _mobileController.ChangingMovingState(isMoving);
        }

        [Method("MoveBackward")]
        public void OnBackwardMoving(bool isMoving)
        {
            if (isMoving)
                _mobileController.SetYDirection(-1);
            else
                _mobileController.SetYDirection(0);

            _mobileController.ChangingMovingState(isMoving);
        }

        [Method("FireLaser")]
        public void FireLaser()
        {
            _mobileController.FireLaser();
        }

        [Method("FireBullets")]
        public void FireBullets()
        {
            _mobileController.FireBullets();
        }
        
        private void OnPanelChangeStateSignal(PanelChangeStateSignal signal)
        {
            if (signal.State)
                IsPanelActive.Value = false;
            else
                IsPanelActive.Value = true;
        }
    }
}