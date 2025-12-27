using System;
using Core.Game;

namespace Core.Phase
{
    public interface IPhaseService<TPhase> where TPhase : struct, Enum
    {
        TPhase Current { get; }

        void Initialize();
        
        void ChangePhase(TPhase next);
        
        void Update(float deltaTime);
    }
}
