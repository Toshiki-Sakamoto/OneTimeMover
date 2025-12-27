using System;

namespace Core.Phase
{
    public interface IPhaseExecutor<TPhase> where TPhase : struct, Enum
    {
        TPhase Phase { get; }
        void Enter(IPhaseContext<TPhase> context);
        void Update(IPhaseContext<TPhase> context, float deltaTime);
        void Exit(IPhaseContext<TPhase> context);
    }
}
