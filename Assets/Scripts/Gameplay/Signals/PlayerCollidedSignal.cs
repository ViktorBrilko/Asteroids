using UnityEngine;

namespace Gameplay.Signals
{
    public class PlayerCollidedSignal
    {
        public GameObject CollidedObject { get; }
        
        public PlayerCollidedSignal(GameObject collidedObject)
        {
            CollidedObject = collidedObject;
        }
    }
}