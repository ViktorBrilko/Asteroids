using System;
using Gameplay.Scores;
using MVVM;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.ViewModels
{
    public class ScoreViewModel : IInitializable, IDisposable
    {
        public readonly  ScoreLogic _scoreLogic;
        [Data("Score")]
        public readonly  ReactiveProperty<string> _currentScore = new();

        public ScoreViewModel(ScoreLogic scoreLogic)
        {
            _scoreLogic = scoreLogic;
        }

        public void Initialize()
        {
            OnScoreChanged(_scoreLogic.Score);
            _scoreLogic.OnStateChanged += OnScoreChanged;
        }

        public void Dispose()
        {
            _scoreLogic.OnStateChanged -= OnScoreChanged;
        }
        
        private void OnScoreChanged(int score)
        {
            _currentScore.Value = score.ToString();
        }
    }
}
