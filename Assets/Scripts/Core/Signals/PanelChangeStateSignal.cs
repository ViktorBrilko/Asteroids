namespace Core.Signals
{
    public class PanelChangeStateSignal
    {
        public PanelChangeStateSignal(bool state)
        {
            State = state;
        }

        public bool State { get; }
    }
}