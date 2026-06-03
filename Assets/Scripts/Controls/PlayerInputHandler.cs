using System;
using Cysharp.Threading.Tasks;

namespace Controls
{
    public class PlayerInputHandler
    {
        public Func<bool, bool, UniTask> ChangeSpeed;
        public Action FireBullet;
        public Action FireLaser;
        public Action StopCompensateInertion;
        public Func<bool, UniTask> StartMovement;
        public Func<UniTaskVoid> InertialMovement;
      //  public Func<bool, bool, UniTask> InertionCompensation;

        public float XDirection { get; set; }

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

        public void TriggerChangeSpeed(bool increase, bool forward = true)
        {
            ChangeSpeed?.Invoke(increase, forward);
        }
        
        // public void TriggerInertionCompensation(bool increase, bool forward)
        // {
        //     InertionCompensation?.Invoke(increase, forward);
        // }
        
        
        
        public void TriggerStopCompensateInertion()
        {
            StopCompensateInertion?.Invoke();
        }
    }
}