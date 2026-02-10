using Gameplay.Base;
using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private Player _player;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        private void Update()
        {
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
                _player.Fire();
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