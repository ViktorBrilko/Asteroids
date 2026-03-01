using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Base
{
    public class EnemyMoveService : MonoBehaviour
    {
        private float _moveSpeed;
        private float _afterCollisionSpeed;
        private int _collisionEffectTime;
        private Vector3 _direction;

        public void Init(float moveSpeed, float afterCollisionSpeed, int collisionEffectTime)
        {
            _moveSpeed = moveSpeed;
            _afterCollisionSpeed = afterCollisionSpeed;
            _collisionEffectTime = collisionEffectTime;

            StartRegularMovement();
        }

        private void Update()
        {
            Move(_direction);
        }

        public void CancelRegularMovement()
        {
            _direction = Vector3.zero;
        }
        
        public void StartRegularMovement()
        {
            _direction = Vector3.up;
        }

        private void Move(Vector3 direction)
        {
            transform.Translate(direction * _moveSpeed * Time.deltaTime, Space.Self);
        }

        public void ChangeMoveDirection(Vector3 direction)
        {
            _direction = direction;
        }

        public async UniTaskVoid ChangeMoveSpeed()
        {
            _moveSpeed = _afterCollisionSpeed;
            await UniTask.Delay(_collisionEffectTime);
            _moveSpeed = _afterCollisionSpeed;
        }
    }
}