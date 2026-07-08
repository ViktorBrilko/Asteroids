using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Controls
{
    public class DesktopController : ITickable
    {
        private readonly PlayerActionCommands _actionCommands;

        public DesktopController(PlayerActionCommands actionCommands)
        {
            _actionCommands = actionCommands;
        }

        public void Tick()
        {
            _actionCommands.YDirection = Input.GetAxisRaw(InputConstants.VerticalMoveAxis);
            _actionCommands.Rotation = Input.GetAxisRaw(InputConstants.RotateAxis);

            if (Input.GetButtonDown(InputConstants.FireBulletButton)) _actionCommands.TriggerBullet();

            if (Input.GetButtonDown(InputConstants.FireLaserButton)) _actionCommands.TriggerLaser();

            if (Input.GetButtonDown(InputConstants.VerticalMoveAxis))
            {
                var forward = _actionCommands.YDirection > 0;

                _actionCommands.TriggerStartMovement(forward).Forget(exception =>
                {
                    if (exception is OperationCanceledException)
                        return;

                    Debug.LogException(exception);
                });
            }

            if (Input.GetButtonUp(InputConstants.VerticalMoveAxis))
            {
                if (Input.GetButton(InputConstants.VerticalMoveAxis)) return;

                _actionCommands.TriggerInertialMovement();
                _actionCommands.TriggerStopCompensateInertia();
                
                _actionCommands.TriggerChangeSpeed(false).Forget(exception =>
                {
                    if (exception is OperationCanceledException)
                        return;

                    Debug.LogException(exception);
                });
            }
        }
    }
}