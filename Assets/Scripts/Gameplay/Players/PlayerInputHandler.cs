using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    public class PlayerInputHandler : ITickable
    {
        private PlayerWeapon _playerWeapon;
        private PlayerMovement _playerMovement;

        public PlayerInputHandler(Player player)
        {
            _playerWeapon = player.GetComponent<PlayerWeapon>();
            _playerMovement = player.GetComponent<PlayerMovement>();
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
            if (_playerMovement.IsUncontrollable) return;

            float xDirection = Input.GetAxisRaw("Horizontal");
            float yDirection = Input.GetAxisRaw("Vertical");

            if (Input.GetButtonDown("Vertical") ||
                Input.GetButtonDown("Horizontal"))
            {
                _playerMovement.ChangeSpeed(true);
            }

            if (Input.GetButton("Horizontal") ||
                Input.GetButton("Vertical"))
            {
                _playerMovement.Move(new Vector3(xDirection, yDirection, 0));
            }

            if (Input.GetButtonUp("Vertical") ||
                Input.GetButtonUp("Horizontal"))
            {
                if (IsMovingButtonsHold()) return;

                _playerMovement.ChangeSpeed(false);
                _playerMovement.InertialMove();
            }

            if (Input.GetButtonDown("Fire1"))
            {
                await _playerWeapon.FireBullets();
            }

            if (Input.GetButtonDown("Fire2"))
            {
                _playerWeapon.FireLaser();
            }

            float rotation = Input.GetAxisRaw("Rotate");
            
            if (rotation != 0)
            {
                _playerMovement.Rotate(rotation);
            }
           
        }
    }
}