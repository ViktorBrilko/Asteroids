using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Controls
{
    public class MobileController
    {
        private readonly PlayerActionCommands _actionCommands;

        public MobileController(PlayerActionCommands actionCommands)
        {
            _actionCommands = actionCommands;
        }

        public void FireLaser()
        {
            _actionCommands.TriggerLaser();
        }

        public void FireBullets()
        {
            _actionCommands.TriggerBullet();
        }

        public void RotateLeft(bool rotate)
        {
            if (rotate)
                _actionCommands.Rotation = 1;
            else
                _actionCommands.Rotation = 0;
        }

        public void RotateRight(bool rotate)
        {
            if (rotate)
                _actionCommands.Rotation = -1;
            else
                _actionCommands.Rotation = 0;
        }

        public void SetYDirection(float y)
        {
            _actionCommands.YDirection = y;
        }

        public async UniTask ChangingMovingState(bool state)
        {
            if (state)
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
            else
            {
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