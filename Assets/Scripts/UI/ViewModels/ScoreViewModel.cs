using System;
using Gameplay.Scores;
using MVVM;
using UniRx;
using Zenject;

namespace UI.ViewModels
{
    public class ScoreViewModel : IInitializable, IDisposable
    {
        public readonly ScoreLogic ScoreLogic;
        [Data("Score")]
        public readonly  ReactiveProperty<string> _currentScore = new();

        public ScoreViewModel(ScoreLogic scoreLogic)
        {
            ScoreLogic = scoreLogic;
        }

        public void Initialize()
        {
            OnScoreChanged(ScoreLogic.Score);
            ScoreLogic.OnScoreChanged += OnScoreChanged;
        }

        public void Dispose()
        {
            ScoreLogic.OnScoreChanged -= OnScoreChanged;
        }
        
        private void OnScoreChanged(int score)
        {
            _currentScore.Value = score.ToString();
        }
    }
}
