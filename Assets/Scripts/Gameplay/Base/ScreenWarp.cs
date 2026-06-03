using Gameplay.Gamefields;
using UnityEngine;
using Zenject;

namespace Gameplay.Base
{
    public class ScreenWarp : MonoBehaviour
    {
        private GameField _field;

        [Inject]
        public void Construct(GameField field)
        {
            _field = field;
        }

        public void Warp()
        {
            var position = transform.position;
            var bounds = _field.GetComponent<Collider2D>().bounds;

            if (position.x > bounds.max.x)
                transform.position = new Vector3(-transform.position.x + 1, transform.position.y);
            else if (position.x < bounds.min.x)
                transform.position = new Vector3(-transform.position.x - 1, transform.position.y);
            else if (position.y > bounds.max.y)
                transform.position = new Vector3(transform.position.x, -transform.position.y + 1);
            else if (position.y < bounds.min.y)
                transform.position = new Vector3(transform.position.x, -transform.position.y - 1);
        }
    }
}