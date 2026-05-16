using UnityEngine;
using Zenject;

namespace Controls
{
    public class DesktopController : ITickable
    {
        private PlayerInputHandler _inputHandler;

        public DesktopController(PlayerInputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }

        public void Tick()
        {
            _inputHandler.XDirection = Input.GetAxisRaw("Horizontal");
            _inputHandler.YDirection = Input.GetAxisRaw("Vertical"); 
            _inputHandler.Rotation = Input.GetAxisRaw("Rotate");
            
            if (Input.GetButtonDown("Fire1"))
            {
                _inputHandler.TriggerBullet();
            }

            if (Input.GetButtonDown("Fire2"))
            {
                 _inputHandler.TriggerLaser();
            }

            if (Input.GetButtonDown("Vertical") ||
                Input.GetButtonDown("Horizontal"))
            {
                _inputHandler.TriggerChangeSpeed(true);
            }

            if (Input.GetButtonUp("Vertical") ||
                Input.GetButtonUp("Horizontal"))
            {
                if (IsMovingButtonsHold()) return;
                
                _inputHandler.TriggerChangeSpeed(false);
                _inputHandler.TriggerInertialMovement();
            }
        }
        
        private bool IsMovingButtonsHold()
        {
            if (Input.GetButton("Vertical") ||
                Input.GetButton("Horizontal"))
            {
                return true;
            }

            return false;
        }
    }
}