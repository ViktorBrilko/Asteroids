using System;
using System.Collections.Generic;
using Gameplay.Base;
using Gameplay.Enemies;
using Gameplay.Signals;
using Zenject;

namespace Gameplay.Scores
{
    public class ScoreLogic : IInitializable, IDisposable
    {
        private int _score;
        private SignalBus _signalBus;
        private Dictionary<EnemyTypes, int> _enemyScoreRates = new();

        public int Score => _score;

        public event Action<int> OnScoreChanged;

        public Dictionary<EnemyTypes, int> EnemyScoreRates
        {
            get => _enemyScoreRates;
            set => _enemyScoreRates = value;
        }

        public ScoreLogic(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<EnemyDiedSignal>(OnEnemyDeath);
        }

        private void OnEnemyDeath(EnemyDiedSignal signal)
        {
            if (signal.Enemy is Enemy enemy)
            {
                _score += _enemyScoreRates[enemy.Type];
                OnScoreChanged?.Invoke(_score);
            }
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<EnemyDiedSignal>(OnEnemyDeath);
        }
    }
}