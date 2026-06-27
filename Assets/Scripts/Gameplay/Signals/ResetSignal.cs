using Gameplay.Base;

namespace Gameplay.Signals
{
    public class ResetSignal<T>
        where T : IResettable
    {
        public T Resetable { get; }
        
        public ResetSignal(T resetable)
        {
            Resetable = resetable;
        }
    }
}