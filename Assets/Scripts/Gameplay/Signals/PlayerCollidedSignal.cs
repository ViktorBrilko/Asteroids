using UnityEngine;

namespace Gameplay.Signals
{
    public class PlayerCollidedSignal
    {
        public PlayerCollidedSignal(GameObject collidedObject)
        {
            CollidedObject = collidedObject;
        }

        public GameObject CollidedObject { get; }
    }
}