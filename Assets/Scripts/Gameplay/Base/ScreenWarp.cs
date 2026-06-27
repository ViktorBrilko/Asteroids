using Gameplay.Gamefields;
using UnityEngine;
using Zenject;

namespace Gameplay.Base
{
    public class ScreenWarp : MonoBehaviour
    {
        private GameField _field;
        private const int WarpThreshold = 1;

        [Inject]
        public void Construct(GameField field)
        {
            _field = field;
        }

        public void Warp()
        {
            var position = transform.position;
            var bounds = _field.Collider.bounds;

            if (position.x > bounds.max.x)
                transform.position = new Vector3(-transform.position.x + WarpThreshold, transform.position.y);
            else if (position.x < bounds.min.x)
                transform.position = new Vector3(-transform.position.x - WarpThreshold, transform.position.y);
            else if (position.y > bounds.max.y)
                transform.position = new Vector3(transform.position.x, -transform.position.y + WarpThreshold);
            else if (position.y < bounds.min.y)
                transform.position = new Vector3(transform.position.x, -transform.position.y - WarpThreshold);
        }
    }
}