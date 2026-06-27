using UnityEngine;
using Zenject;

namespace Controls
{
    public class DesktopController : ITickable
    {
        private const string FireBulletButton = "Fire1";
        private const string FireLaserButton = "Fire2";
        private const string RotateAxis = "Rotate";
        private const string VerticalMoveAxis = "Vertical";
        
        private readonly PlayerActionCommands _actionCommands;

        public DesktopController(PlayerActionCommands actionCommands)
        {
            _actionCommands = actionCommands;
        }

        public void Tick()
        {
            _actionCommands.YDirection = Input.GetAxisRaw(VerticalMoveAxis);
            _actionCommands.Rotation = Input.GetAxisRaw(RotateAxis);

            if (Input.GetButtonDown(FireBulletButton)) _actionCommands.TriggerBullet();

            if (Input.GetButtonDown(FireLaserButton)) _actionCommands.TriggerLaser();

            if (Input.GetButtonDown(VerticalMoveAxis))
            {
                var forward = _actionCommands.YDirection > 0;

                _actionCommands.TriggerStartMovement(forward);
            }

            if (Input.GetButtonUp(VerticalMoveAxis))
            {
                if (IsMovingButtonsHold()) return;

                _actionCommands.TriggerInertialMovement();
                _actionCommands.TriggerStopCompensateInertia();
                _actionCommands.TriggerChangeSpeed(false);
            }
        }

        private bool IsMovingButtonsHold()
        {
            return Input.GetButton(VerticalMoveAxis);
        }
    }
}