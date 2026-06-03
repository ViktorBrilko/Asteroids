using Gameplay.Base;

namespace Gameplay.Signals
{
    public class ResetSignal<T>
        where T : IResetable
    {
        public ResetSignal(T resetable)
        {
            Resetable = resetable;
        }

        public T Resetable { get; }
    }
}