using System;
using Core.Signals;
using UnityEngine;
using Zenject;

namespace Core
{
    public class PauseService : IInitializable, IDisposable
    {
        SignalBus _signalBus;

        public PauseService(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
            _signalBus.Subscribe<PauseGameSignal>(PauseGame);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PauseGameSignal>(PauseGame);
        }

        private void PauseGame(PauseGameSignal signal)
        {
            Time.timeScale = signal.Pause ? 0 : 1;
        }
    }
}