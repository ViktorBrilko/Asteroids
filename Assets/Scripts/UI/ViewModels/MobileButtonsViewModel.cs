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

        public void Initialize()
        {
        }

        public void Dispose()
        {
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
    }
}