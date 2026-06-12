namespace Controls
{
    public class MobileController
    {
        private readonly PlayerInputHandler _inputHandler;

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
                _inputHandler.Rotation = 1;
            else
                _inputHandler.Rotation = 0;
        }

        public void RotateRight(bool rotate)
        {
            if (rotate)
                _inputHandler.Rotation = -1;
            else
                _inputHandler.Rotation = 0;
        }

        public void SetYDirection(float y)
        {
            _inputHandler.YDirection = y;
        }

        public void ChangingMovingState(bool state)
        {
            if (state)
            {
                var forward = _inputHandler.YDirection > 0;

                _inputHandler.TriggerStartMovement(forward);
            }
            else
            {
                _inputHandler.TriggerInertialMovement();
                _inputHandler.TriggerStopCompensateInertion();
                _inputHandler.TriggerChangeSpeed(false);
            }
        }
    }
}