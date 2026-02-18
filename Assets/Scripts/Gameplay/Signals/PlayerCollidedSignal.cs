using UnityEngine;

namespace Gameplay.Signals
{
    public class PlayerCollidedSignal
    {
        private GameObject _collidedObject;

        public GameObject CollidedObject => _collidedObject;

        public PlayerCollidedSignal(GameObject collidedObject)
        {
            _collidedObject = collidedObject;
        }
    }
}