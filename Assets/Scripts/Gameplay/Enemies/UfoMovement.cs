using UnityEngine;
using Zenject;

namespace Gameplay.Enemies
{
    public class UfoMovement : EnemyMoveService
    {
        private bool _isChasing;

        public Transform Player { get; set; }

        private void Update()
        {
            Move(Direction);

            if (_isChasing) Chasing();
        }

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

        private void Chasing()
        {
            transform.up = Player.position - transform.position;
        }
    }
}