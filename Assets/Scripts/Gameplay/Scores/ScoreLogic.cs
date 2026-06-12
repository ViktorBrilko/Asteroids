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

        public ScoreLogic(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public int Score { get; private set; }

        public Dictionary<EnemyTypes, int> EnemyScoreRates { get; set; } = new();

        public void Dispose()
        {
            _signalBus.Unsubscribe<EnemyDiedSignal>(OnEnemyDeath);
        }

        public void Initialize()
        {
            _signalBus.Subscribe<EnemyDiedSignal>(OnEnemyDeath);
        }

        public event Action<int> OnScoreChanged;

        private void OnEnemyDeath(EnemyDiedSignal signal)
        {
            if (signal.Enemy is Enemy enemy)
            {
                Score += EnemyScoreRates[enemy.Type];
                OnScoreChanged?.Invoke(Score);
            }
        }
    }
}