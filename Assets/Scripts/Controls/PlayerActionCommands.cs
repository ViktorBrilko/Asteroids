using System;
using Cysharp.Threading.Tasks;

namespace Controls
{
    public class PlayerActionCommands
    {
        public event Func<bool, UniTask> ChangeSpeed;
        public event Action FireBullet;
        public event Action FireLaser;
        public event Action InertialMovement;
        public event Func<bool, UniTask> StartMovement;
        public event Action StopCompensateInertia;

        public float YDirection { get; set; }

        public float Rotation { get; set; }

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

        public void TriggerStartMovement(bool isForward)
        {
            StartMovement?.Invoke(isForward);
        }

        public void TriggerChangeSpeed(bool increase)
        {
            ChangeSpeed?.Invoke(increase);
        }

        public void TriggerStopCompensateInertia()
        {
            StopCompensateInertia?.Invoke();
        }
    }
}