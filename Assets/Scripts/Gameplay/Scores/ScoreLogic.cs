using System;
using System.Collections.Generic;
using Gameplay.Enemies;
using Gameplay.Signals;
using Zenject;

namespace Gameplay.Scores
{
    public class ScoreLogic : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private Dictionary<EnemyType, int> _enemyScoreRates;

        public int Score { get; private set; }
        
        public event Action<int> OnScoreChanged;
        
        public ScoreLogic(SignalBus signalBus, Dictionary<EnemyType, int> enemyScoreRates)
        {
            _signalBus = signalBus;
            _enemyScoreRates = enemyScoreRates;
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<EnemyDiedSignal>(OnEnemyDeath);
        }

        public void Initialize()
        {
            _signalBus.Subscribe<EnemyDiedSignal>(OnEnemyDeath);
        }

        private void OnEnemyDeath(EnemyDiedSignal signal)
        {
            if (signal.Enemy is Enemy enemy)
            {
                Score += _enemyScoreRates[enemy.Type];
                OnScoreChanged?.Invoke(Score);
            }
        }
    }
}