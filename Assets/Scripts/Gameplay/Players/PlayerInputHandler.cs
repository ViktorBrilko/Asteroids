using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    public class PlayerInputHandler : ITickable
    {
        private Player _player;
        private float _lastXDirection;

        public PlayerInputHandler(Player player)
        {
            _player = player;
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

        public async void Tick()
        {
            if (_player.IsUncontrollable) return;

            float xDirection = Input.GetAxisRaw("Horizontal");
            float yDirection = Input.GetAxisRaw("Vertical");

            if (Input.GetButtonDown("Vertical") ||
                Input.GetButtonDown("Horizontal"))
            {
                _player.ChangeSpeed(true);
            }

            if (Input.GetButton("Horizontal") ||
                Input.GetButton("Vertical"))
            {
                _player.Move(xDirection, yDirection);
            }

            if (Input.GetButtonUp("Vertical") ||
                Input.GetButtonUp("Horizontal"))
            {
                if (IsMovingButtonsHold()) return;

                _player.ChangeSpeed(false);
                _player.InertialMove();
            }

            if (Input.GetButtonDown("Fire1"))
            {
                await _player.FireBullets();
            }

            if (Input.GetButtonDown("Fire2"))
            {
                _player.FireLaser();
            }

            float rotation = Input.GetAxisRaw("Rotate");
            
            if (rotation != 0)
            {
                _player.Rotate(rotation);
            }
           
        }
    }
}