using Core.Phase;

namespace Core.Game
{
    public class GameGamePhaseClearExecutor : PhaseExecutor<GamePhase, IGamePhaseClearHandler>, IGamePhaseClearExecutor
    {
        public override GamePhase Phase => GamePhase.Clear;
    }
}
