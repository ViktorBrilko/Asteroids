using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private Transform _modelTransform;
        private float _afterCollisionSpeed;
        private int _collisionEffectTime;
        private CancellationTokenSource _cts;
        private float _currentSpeed;
        private float _regularSpeed;
        private float _rotationSpeed;

        protected Vector3 Direction;
        
        public void Init(float moveSpeed, float afterCollisionSpeed, int collisionEffectTime, float rotationSpeed)
        {
            _regularSpeed = moveSpeed;
            _rotationSpeed = rotationSpeed;
            _currentSpeed = _regularSpeed;
            _afterCollisionSpeed = afterCollisionSpeed;
            _collisionEffectTime = collisionEffectTime;

            StartRegularMovement();
        }
        
        public async UniTaskVoid ChangeMoveDirection(Vector3 direction)
        {
            try
            {
                Direction = direction;
                await UniTask.Delay(TimeSpan.FromSeconds(_collisionEffectTime), cancellationToken: _cts.Token);
                StartRegularMovement();
            }
            catch (OperationCanceledException e)
            {
                Debug.Log(e.Message);
            }
        }

        public async UniTask RotateAfterCollision()
        {
            float elapsedTime = 0;

            try
            {
                while (elapsedTime < _collisionEffectTime)
                {
                    _modelTransform.transform.RotateAround(_modelTransform.transform.position, Vector3.forward,
                        _rotationSpeed * Time.deltaTime);
                    elapsedTime += Time.deltaTime;
                    await UniTask.NextFrame(_cts.Token);
                }

                _modelTransform.transform.localEulerAngles = Vector3.zero;
            }
            catch (OperationCanceledException e)
            {
                Debug.Log(e.Message);
            }
        }

        public async UniTask ChangeMoveSpeed()
        {
            try
            {
                _currentSpeed = _afterCollisionSpeed;
                await UniTask.Delay(TimeSpan.FromSeconds(_collisionEffectTime), cancellationToken: _cts.Token);
                _currentSpeed = _regularSpeed;
            }
            catch (OperationCanceledException e)
            {
                Debug.Log(e.Message);
            }
        }

        protected void StartRegularMovement()
        {
            Direction = Vector3.up;
        }
        
        protected void Move(Vector3 direction)
        {
            transform.Translate(direction * _currentSpeed * Time.deltaTime, Space.Self);
        }

        private void Update()
        {
            Move(Direction);
        }

        private void OnEnable()
        {
            _cts = new CancellationTokenSource();
        }

        private void OnDisable()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }
        }
    }
}