using System;

namespace Core.Phase
{
    public interface IPhaseContext<TPhase> where TPhase : struct, Enum
    {
        TPhase Current { get; }
        void RequestChange(TPhase next);
        bool HasPendingChange { get; }
    }
}
