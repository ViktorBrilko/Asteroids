using UnityEngine;
using Zenject;

namespace Controls
{
    public class DesktopController : ITickable
    {
        private readonly PlayerInputHandler _inputHandler;
        //private float _lastYDirection;

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
                bool forward = _inputHandler.YDirection > 0;

                _inputHandler.TriggerStartMovement(forward);

                // if (Mathf.Approximately(_inputHandler.YDirection, _lastYDirection) ||
                //     Mathf.Approximately(0, _lastYDirection))
                // {
                //     _inputHandler.TriggerChangeSpeed(true, forward);
                //     Debug.Log("совпадает");
                // }
                // else
                // {
                //     Debug.Log("не совпадает");
                //     _inputHandler.TriggerInertionCompensation(true, forward);
                // }

                // _lastYDirection = _inputHandler.YDirection;
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