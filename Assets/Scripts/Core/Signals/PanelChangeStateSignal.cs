namespace Core.Signals
{
    public class PanelChangeStateSignal
    {
        public bool State { get; }
        
        public PanelChangeStateSignal(bool state)
        {
            State = state;
        }
    }
}