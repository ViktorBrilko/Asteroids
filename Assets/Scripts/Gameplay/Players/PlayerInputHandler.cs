using Cysharp.Threading.Tasks;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private Player _player;
        private SignalBus _signalBus;
        private bool _isUncontrollable;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        //TODO перенести в Construct
        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        public void OnEnable()
        {
            _signalBus.Subscribe<PlayerCollidedSignal>(OnPlayerCollision);
        }

        public void OnDisable()
        {
            _signalBus.Unsubscribe<PlayerCollidedSignal>(OnPlayerCollision);
        }

        private async void OnPlayerCollision(PlayerCollidedSignal signal)
        {
            float elapsedTime = 0;
            _isUncontrollable = true;
            var direction = (transform.position - signal.CollidedObject.transform.position).normalized;

            while (elapsedTime < _player.PlayerConfig.UncontrollableTime)
            {
                _player.MoveAfterCollision(direction);
                elapsedTime += Time.deltaTime;
                await UniTask.NextFrame();
            }

            _isUncontrollable = false;
        }

        private async void Update()
        {
            if (!_isUncontrollable)
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
                    await _player.FireBullets();
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
}