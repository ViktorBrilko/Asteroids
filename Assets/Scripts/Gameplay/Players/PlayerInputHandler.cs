using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    public class PlayerInputHandler : ITickable
    {
        private Player _player;

        public PlayerInputHandler(Player player)
        {
            _player = player;
        }

        public async void Tick()
        {
            if (_player.IsUncontrollable) return;
            
            float xDirection = Input.GetAxis("Horizontal");
            float yDirection = Input.GetAxis("Vertical");

            if (xDirection != 0)
            {
                _player.HorizontalMove(xDirection);
            }

            if (yDirection != 0)
            {
                _player.VerticalMove(yDirection);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                await _player.FireBullets();
            }
                
            if (Input.GetKeyDown(KeyCode.R))
            {
                _player.FireLaser();
            }

            if (Input.GetKey(KeyCode.E))
            {
                _player.Rotate(true);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                _player.Rotate(false);
            }
        }
    }
}