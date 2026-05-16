using System;

namespace Controls
{
    public class PlayerInputHandler
    {
        public Action FireLaser;
        public Action FireBullet;
        public Action<bool> ChangeSpeed;
        public Action InertialMovement;

        private float _xDirection;
        private float _yDirection;
        private float _rotation;

        public float XDirection
        {
            get => _xDirection;
            set => _xDirection = value;
        }

        public float YDirection
        {
            get => _yDirection;
            set => _yDirection = value;
        }

        public float Rotation
        {
            get => _rotation;
            set => _rotation = value;
        }

        public void TriggerLaser()
        {
            FireLaser?.Invoke();
        }

        public void TriggerBullet()
        {
            FireBullet?.Invoke();
        }
        
        public void TriggerInertialMovement()
        {
            InertialMovement?.Invoke();
        }
        
        public void TriggerChangeSpeed(bool increase)
        {
            ChangeSpeed?.Invoke(increase);
        }
        
    }
}