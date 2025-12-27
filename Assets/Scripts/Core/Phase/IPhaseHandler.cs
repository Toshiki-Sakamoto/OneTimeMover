using System;

namespace Core.Phase
{
    /// <summary>
    /// フェーズごとの共通ライフサイクル（任意のenumフェーズに対応）。
    /// </summary>
    public interface IPhaseHandler<TPhase> where TPhase : struct, Enum
    {
        int Priority => 0;
        
        void OnEnter(IPhaseContext<TPhase> context);
        void OnUpdate(IPhaseContext<TPhase> context, float deltaTime);
        void OnExit(IPhaseContext<TPhase> context);
    }
}
