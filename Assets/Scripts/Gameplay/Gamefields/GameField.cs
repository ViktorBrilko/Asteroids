using Core.Configs;
using UnityEngine;
using Zenject;

namespace Gameplay.Gamefields
{
    public class GameField : MonoBehaviour
    {
        private GameFieldConfig _config;
        private Vector3 _startPosition;
        private BoxCollider2D _collider;

        public BoxCollider2D Collider => _collider;

        [Inject]
        public void Construct(GameFieldConfig config)
        {
            _config = config;
        }

        private void Start()
        {
            _collider = GetComponent<BoxCollider2D>();
            _collider.size = new Vector3(_config.XSize, _config.YSize, 0);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(_config.XSize, _config.YSize, 0));
        }
#endif

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out ScreenWarp screenWarp))
            {
                screenWarp.Warp();
            }
        }
    }
}