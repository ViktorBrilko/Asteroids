using System;
using Gameplay.Scores;
using MVVM;
using UniRx;
using Zenject;

namespace UI.ViewModels
{
    public class ScoreViewModel : IInitializable, IDisposable
    {
        [Data("Score")] public readonly ReactiveProperty<string> _currentScore = new();

        private readonly ScoreLogic _scoreLogic;

        public ScoreViewModel(ScoreLogic scoreLogic)
        {
            _scoreLogic = scoreLogic;
        }

        public void Dispose()
        {
            _scoreLogic.OnScoreChanged -= OnScoreChanged;
        }

        public void Initialize()
        {
            OnScoreChanged(_scoreLogic.Score);
            _scoreLogic.OnScoreChanged += OnScoreChanged;
        }

        private void OnScoreChanged(int score)
        {
            _currentScore.Value = score.ToString();
        }
    }
}