using System;
using System.Collections.Generic;
using Core.Configs;
using Cysharp.Threading.Tasks;
using Gameplay.Enemies;
using Gameplay.Gamefields;
using Gameplay.Signals;
using UnityEngine;
using Zenject;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;

namespace Gameplay.Base
{
    public class EnemyGeneratorService : IInitializable, IDisposable
    {
        private SignalBus _signalBus;
        private Spawner<Asteroid> _asteroidSpawner;
        private Spawner<SmallAsteroid> _smallAsteroidSpawner;
        private Spawner<Ufo> _ufoSpawner;
        private GameFieldConfig _config;
        private GameField _gameField;
        private List<IDieable> _enemies;
        private int _asteroidsCount;
        private int _ufosCount;

        public EnemyGeneratorService(Spawner<Asteroid> asteroidSpawner, GameField gameField, GameFieldConfig config,
            SignalBus signalBus, Spawner<SmallAsteroid> smallAsteroidSpawner, Spawner<Ufo> ufoSpawner)
        {
            _asteroidSpawner = asteroidSpawner;
            _smallAsteroidSpawner = smallAsteroidSpawner;
            _ufoSpawner = ufoSpawner;
            _gameField = gameField;
            _config = config;
            _signalBus = signalBus;
        }

        public async void Initialize()
        {
            _signalBus.Subscribe<EnemyDiedSignal>(OnEnemyDeath);

            while (_asteroidsCount < _config.MaxAsteroids)
            {
                await SpawnAsteroids();
            }

            while (_ufosCount < _config.MaxUfos)
            {
                await SpawnUfos();
            }
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<EnemyDiedSignal>(OnEnemyDeath);
        }

        private async void OnEnemyDeath(EnemyDiedSignal signal)
        {
            if (signal.Enemy is Asteroid _)
            {
                _asteroidsCount--;
                SpawnSmallAsteroids(signal.DeathPosition);

                if (_asteroidsCount < _config.MaxAsteroids)
                {
                    await SpawnAsteroids();
                }
            }
            else if (signal.Enemy is Ufo _)
            {
                _ufosCount--;

                if (_ufosCount < _config.MaxUfos)
                {
                    await SpawnUfos();
                }
            }
        }

        private void SpawnSmallAsteroids(Vector3 position)
        {
            int asteroidsCount = Random.Range(_config.MinSmallAsteroids, _config.MaxSmallAsteroids);

            for (int i = 0; i < asteroidsCount; i++)
            {
                _smallAsteroidSpawner.SpawnItem(position, GetRandomRotation());
            }
        }

        private async UniTask SpawnAsteroids()
        {
            await UniTask.Delay(_config.AsteroidSpawnCooldown);
            Vector3 spawnPosition = GetSpawnPosition();
            _asteroidSpawner.SpawnItem(spawnPosition, GetRandomRotation());
            _asteroidsCount++;
        }

        private async UniTask SpawnUfos()
        {
            await UniTask.Delay(_config.UfoSpawnCooldown);
            Vector3 spawnPosition = GetSpawnPosition();
            _ufoSpawner.SpawnItem(spawnPosition, GetRandomRotation());
            _ufosCount++;
        }

        private Vector3 GetSpawnPosition()
        {
            bool isNotInCameraView = false;
            Vector3 candidatePosition = new Vector3();
            int attempts = 0;

            while (!isNotInCameraView && attempts < _config.MaxAttemptsToPlaceEnemy)
            {
                candidatePosition = new Vector3(
                    Random.Range(_gameField.Collider.bounds.min.x, _gameField.Collider.bounds.max.x),
                    Random.Range(_gameField.Collider.bounds.min.y, _gameField.Collider.bounds.max.y), 0);

                var viewportPosition = Camera.main.WorldToViewportPoint(candidatePosition);

                bool isXOnScreen = viewportPosition.x is < 1 and > 0;
                bool isYOnScreen = viewportPosition.y is < 1 and > 0;

                if (!isXOnScreen && !isYOnScreen)
                {
                    isNotInCameraView = true;
                }

                attempts++;
            }

            if (attempts >= _config.MaxAttemptsToPlaceEnemy)
            {
                Debug.Log("The number of attempts to find the enemy position has expired.");
            }

            return candidatePosition;
        }

        private Quaternion GetRandomRotation()
        {
            int zRotation = Random.Range(0, 360);
            return Quaternion.Euler(0, 0, zRotation);
        }
    }
}