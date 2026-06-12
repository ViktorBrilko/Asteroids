using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class EnemyMoveService : MonoBehaviour
    {
        [SerializeField] private GameObject _model;
        private float _afterCollisionSpeed;
        private int _collisionEffectTime;
        private CancellationTokenSource _cts;
        private float _currentSpeed;

        private float _regularSpeed;
        private float _rotationSpeed;

        protected Vector3 Direction;

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

        public void Init(float moveSpeed, float afterCollisionSpeed, int collisionEffectTime, float rotationSpeed)
        {
            _regularSpeed = moveSpeed;
            _rotationSpeed = rotationSpeed;
            _currentSpeed = _regularSpeed;
            _afterCollisionSpeed = afterCollisionSpeed;
            _collisionEffectTime = collisionEffectTime;

            StartRegularMovement();
        }

        protected void StartRegularMovement()
        {
            Direction = Vector3.up;
        }

        protected void Move(Vector3 direction)
        {
            transform.Translate(direction * _currentSpeed * Time.deltaTime, Space.Self);
        }

        public async void ChangeMoveDirection(Vector3 direction)
        {
            try
            {
                Direction = direction;
                await UniTask.Delay(TimeSpan.FromSeconds(_collisionEffectTime), cancellationToken: _cts.Token);
                StartRegularMovement();
            }
            catch (OperationCanceledException ex)
            {
            }
        }

        public async UniTaskVoid RotateAfterCollision()
        {
            float elapsedTime = 0;

            try
            {
                while (elapsedTime < _collisionEffectTime)
                {
                    _model.transform.RotateAround(_model.transform.position, Vector3.forward,
                        _rotationSpeed * Time.deltaTime);
                    elapsedTime += Time.deltaTime;
                    await UniTask.NextFrame(_cts.Token);
                }

                _model.transform.localEulerAngles = Vector3.zero;
            }
            catch (OperationCanceledException ex)
            {
            }
        }

        public async UniTaskVoid ChangeMoveSpeed()
        {
            try
            {
                _currentSpeed = _afterCollisionSpeed;
                await UniTask.Delay(TimeSpan.FromSeconds(_collisionEffectTime), cancellationToken: _cts.Token);
                _currentSpeed = _regularSpeed;
            }
            catch (OperationCanceledException ex)
            {
            }
        }
    }
}