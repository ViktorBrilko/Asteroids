using System;
using Controls;
using MVVM;
using Zenject;

namespace UI.ViewModels
{
    public class MobileButtonsViewModel : IInitializable, IDisposable
    {
        public readonly MobileController MobileController;

        public MobileButtonsViewModel(MobileController mobileController)
        {
            MobileController = mobileController;
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

        [Setter("XDirection")]
        public float XDirection
        {
            set => MobileController.SetXDirection(value);
        }

        [Setter("YDirection")]
        public float YDirection
        {
            set => MobileController.SetYDirection(value);
        }

        public void Dispose()
        {
        }

        public void Initialize()
        {
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