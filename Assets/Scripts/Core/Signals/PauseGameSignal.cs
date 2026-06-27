namespace Core.Signals
{
    public class PauseGameSignal
    {
        public bool Pause { get; }

        public PauseGameSignal(bool pause)
        {
            Pause = pause;
        }
    }
}