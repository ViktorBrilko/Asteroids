using System;
using Cysharp.Threading.Tasks;

namespace Controls
{
    public class PlayerActionCommands
    {
        public event Func<bool, UniTask> ChangeSpeed;
        public event Func<UniTask> FireBullet;
        public event Func<UniTask> FireLaser;
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

        public async UniTask TriggerStartMovement(bool isForward)
        {
            if (StartMovement != null)
                await StartMovement.Invoke(isForward);
        }

        public async UniTask TriggerChangeSpeed(bool increase)
        {
            if (ChangeSpeed != null)
                await ChangeSpeed.Invoke(increase);
        }

        public void TriggerStopCompensateInertia()
        {
            StopCompensateInertia?.Invoke();
        }
    }
}