using UnityEngine;
using Zenject;

namespace Gameplay.Enemies
{
    public class UfoMovement : EnemyMovement
    {
        private bool _isChasing;
        private Transform _playerTransform;
        
        [Inject]
        public void Construct()
        {
            StartRegularMovement();
        }
        
        public void StartChasing()
        {
            _isChasing = true;
        }

        public void StopChasing()
        {
            _isChasing = false;
        }
        
        public void SetTarget(Transform target)
        {
            _playerTransform = target;
        }
        
        private void Update()
        {
            Move(Direction);

            if (_isChasing) Chasing();
        }
       
        private void Chasing()
        {
            transform.up = _playerTransform.position - transform.position;
        }
    }
}