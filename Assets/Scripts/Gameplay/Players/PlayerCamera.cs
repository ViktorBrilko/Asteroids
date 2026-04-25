using Cinemachine;
using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    public class PlayerCamera : MonoBehaviour
    {
        private Player _player;
        private CinemachineVirtualCamera _camera;

        [Inject]
        public void Construct(Player player)
        {
            _player = player;
            _camera = GetComponent<CinemachineVirtualCamera>();
            _camera.Follow = _player.transform;
        }

       
    }
}