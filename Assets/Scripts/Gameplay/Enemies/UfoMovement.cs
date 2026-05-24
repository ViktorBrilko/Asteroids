using UnityEngine;
using Zenject;

namespace Gameplay.Base
{
    public class UfoMovement : EnemyMoveService
    {
        private Transform _player;
        private bool _isChasing;

        [Inject]
        public void Construct()
        {
            StartRegularMovement();
        }

        public Transform Player
        {
            get => _player;
            set => _player = value;
        }

        private void Update()
        {
            Move(Direction);
            
            if (_isChasing)
            {
               Chasing();
            }
        }

        public void StartChasing()
        {
            _isChasing = true;
        }

        public void StopChasing()
        {
            _isChasing = false;
        }

        private void Chasing()
        {
            transform.up = _player.position - transform.position;
        }
    }
}