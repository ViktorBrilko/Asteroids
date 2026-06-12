using System;
using Cysharp.Threading.Tasks;

namespace Controls
{
    public class PlayerInputHandler
    {
        public Func<bool, UniTask> ChangeSpeed;
        public Action FireBullet;
        public Action FireLaser;
        public Func<UniTaskVoid> InertialMovement;
        public Func<bool, UniTask> StartMovement;
        public Action StopCompensateInertion;

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

        public void TriggerStopCompensateInertion()
        {
            StopCompensateInertion?.Invoke();
        }
    }
}