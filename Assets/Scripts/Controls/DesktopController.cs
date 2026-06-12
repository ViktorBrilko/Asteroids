using UnityEngine;
using Zenject;

namespace Controls
{
    public class DesktopController : ITickable
    {
        private readonly PlayerInputHandler _inputHandler;

        public DesktopController(PlayerInputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }

        public void Tick()
        {
            _inputHandler.YDirection = Input.GetAxisRaw("Vertical");
            _inputHandler.Rotation = Input.GetAxisRaw("Rotate");

            if (Input.GetButtonDown("Fire1")) _inputHandler.TriggerBullet();

            if (Input.GetButtonDown("Fire2")) _inputHandler.TriggerLaser();

            if (Input.GetButtonDown("Vertical"))
            {
                var forward = _inputHandler.YDirection > 0;

                _inputHandler.TriggerStartMovement(forward);
            }

            if (Input.GetButtonUp("Vertical"))
            {
                if (IsMovingButtonsHold()) return;

                _inputHandler.TriggerInertialMovement();
                _inputHandler.TriggerStopCompensateInertion();
                _inputHandler.TriggerChangeSpeed(false);
            }
        }

        private bool IsMovingButtonsHold()
        {
            if (Input.GetButton("Vertical"))
                return true;

            return false;
        }
    }
}