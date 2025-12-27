using Core.Phase;

namespace Core.Game
{
    public class GameGamePhaseInitializeExecutor : PhaseExecutor<GamePhase, IGamePhaseInitializeHandler>, IGamePhaseInitializeExecutor
    {
        public override GamePhase Phase => GamePhase.Initialize;
    }
}