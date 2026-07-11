using System;
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

        public async void Tick()
        {
            _actionCommands.YDirection = Input.GetAxisRaw(InputConstants.VerticalMoveAxis);
            _actionCommands.Rotation = Input.GetAxisRaw(InputConstants.RotateAxis);

            if (Input.GetButtonDown(InputConstants.FireBulletButton)) _actionCommands.TriggerBullet();

            if (Input.GetButtonDown(InputConstants.FireLaserButton)) _actionCommands.TriggerLaser();

            if (Input.GetButtonDown(InputConstants.VerticalMoveAxis))
            {
                var forward = _actionCommands.YDirection > 0;

                try
                {
                    await _actionCommands.TriggerStartMovement(forward);
                }
                catch (OperationCanceledException)
                {
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            if (Input.GetButtonUp(InputConstants.VerticalMoveAxis))
            {
                if (Input.GetButton(InputConstants.VerticalMoveAxis)) return;

                _actionCommands.TriggerInertialMovement();
                _actionCommands.TriggerStopCompensateInertia();

                try
                {
                    await _actionCommands.TriggerChangeSpeed(false);
                }
                catch (OperationCanceledException)
                {
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}