using Cinemachine;
using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    public class PlayerCamera : MonoBehaviour
    {
        private CinemachineVirtualCamera _camera;
        private Player _player;

        [Inject]
        public void Construct(Player player)
        {
            _player = player;
            _camera = GetComponent<CinemachineVirtualCamera>();
            _camera.Follow = _player.transform;
        }
    }
}