namespace Controls
{
    public class MobileController
    {
        private PlayerInputHandler _inputHandler;

        public MobileController(PlayerInputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }

        public void FireLaser()
        {
            _inputHandler.TriggerLaser();
        }

        public void FireBullets()
        {
            _inputHandler.TriggerBullet();
        }

        public void RotateLeft(bool rotate)
        {
            if (rotate)
            {
                _inputHandler.Rotation = 1;
            }
            else
            {
                _inputHandler.Rotation = 0;
            }
        }

        public void RotateRight(bool rotate)
        {
            if (rotate)
            {
                _inputHandler.Rotation = -1;
            }
            else
            {
                _inputHandler.Rotation = 0;
            }
        }

        public void SetXDirection(float x)
        {
            _inputHandler.XDirection = x;
        }

        public void SetYDirection(float y)
        {
            _inputHandler.YDirection = y;
        }

        public void ChangingMovingState(bool state)
        {

            if (state)
            {
                _inputHandler.ChangeSpeed?.Invoke(true);
            }
            else
            {
                _inputHandler.ChangeSpeed?.Invoke(false);
                _inputHandler.InertialMovement?.Invoke();
            }
        }
    }
}