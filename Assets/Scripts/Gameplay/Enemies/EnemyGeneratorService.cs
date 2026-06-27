using System;
using System.Threading;
using Core.Configs;
using Cysharp.Threading.Tasks;
using Gameplay.Base;
using Gameplay.Gamefields;
using Gameplay.Signals;
using UnityEngine;
using Zenject;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;

namespace Gameplay.Enemies
{
    public class EnemyGeneratorService : IInitializable, IDisposable
    {
        private readonly Spawner<Asteroid> _asteroidSpawner;
        private readonly GameFieldConfig _config;
        private readonly GameField _gameField;
        private readonly SignalBus _signalBus;
        private readonly Spawner<SmallAsteroid> _smallAsteroidSpawner;
        private readonly Spawner<Ufo> _ufoSpawner;
        private int _asteroidsCount;
        private CancellationTokenSource _cts;
        private int _ufosCount;
        private Camera _camera;

        public EnemyGeneratorService(Spawner<Asteroid> asteroidSpawner, GameField gameField, GameFieldConfig config,
            SignalBus signalBus, Spawner<SmallAsteroid> smallAsteroidSpawner, Spawner<Ufo> ufoSpawner, Camera camera)
        {
            _asteroidSpawner = asteroidSpawner;
            _smallAsteroidSpawner = smallAsteroidSpawner;
            _ufoSpawner = ufoSpawner;
            _gameField = gameField;
            _camera = camera;

            _config = config;
            _signalBus = signalBus;
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<EnemyDiedSignal>(OnEnemyDeath);

            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }
        }

        public void Initialize()
        {
            _cts = new CancellationTokenSource();

            _signalBus.Subscribe<EnemyDiedSignal>(OnEnemyDeath);

            SpawnUfos(_cts.Token).Forget();
            SpawnAsteroids(_cts.Token).Forget();
        }

        private async void OnEnemyDeath(EnemyDiedSignal signal)
        {
            if (signal.Enemy is Asteroid _)
            {
                _asteroidsCount--;
                SpawnSmallAsteroids(signal.DeathPosition);

                if (_asteroidsCount < _config.MaxAsteroids) await SpawnAsteroids(_cts.Token);
            }
            else if (signal.Enemy is Ufo _)
            {
                _ufosCount--;

                if (_ufosCount < _config.MaxUfos) await SpawnUfos(_cts.Token);
            }
        }

        private void SpawnSmallAsteroids(Vector3 position)
        {
            var asteroidsCount = Random.Range(_config.MinSmallAsteroids, _config.MaxSmallAsteroids);

            for (var i = 0; i < asteroidsCount; i++) _smallAsteroidSpawner.SpawnItem(position, GetRandomRotation());
        }

        private async UniTask SpawnAsteroids(CancellationToken cancellationToken)
        {
            try
            {
                while (_asteroidsCount < _config.MaxAsteroids)
                {
                    await UniTask.Delay(_config.AsteroidSpawnCooldown, cancellationToken: cancellationToken);
                    var spawnPosition = GetSpawnPosition();
                    _asteroidSpawner.SpawnItem(spawnPosition, GetRandomRotation());
                    _asteroidsCount++;
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private async UniTask SpawnUfos(CancellationToken cancellationToken)
        {
            try
            {
                while (_ufosCount < _config.MaxUfos)
                {
                    await UniTask.Delay(_config.UfoSpawnCooldown, cancellationToken: cancellationToken);
                    var spawnPosition = GetSpawnPosition();
                    _ufoSpawner.SpawnItem(spawnPosition, GetRandomRotation());
                    _ufosCount++;
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private Vector3 GetSpawnPosition()
        {
            var isNotInCameraView = false;
            var candidatePosition = new Vector3();
            var attempts = 0;

            while (!isNotInCameraView && attempts < _config.MaxAttemptsToPlaceEnemy)
            {
                candidatePosition = new Vector3(
                    Random.Range(_gameField.Collider.bounds.min.x, _gameField.Collider.bounds.max.x),
                    Random.Range(_gameField.Collider.bounds.min.y, _gameField.Collider.bounds.max.y), 0);

                var viewportPosition = _camera.WorldToViewportPoint(candidatePosition);

                var isXOnScreen = viewportPosition.x is < 1 and > 0;
                var isYOnScreen = viewportPosition.y is < 1 and > 0;

                if (!isXOnScreen && !isYOnScreen) isNotInCameraView = true;

                attempts++;
            }

            if (attempts >= _config.MaxAttemptsToPlaceEnemy)
                Debug.Log("The number of attempts to find the enemy position has expired.");

            return candidatePosition;
        }

        private Quaternion GetRandomRotation()
        {
            var zRotation = Random.Range(0, 360);
            return Quaternion.Euler(0, 0, zRotation);
        }
    }
}