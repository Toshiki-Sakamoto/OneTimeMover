using Core.Phase;

namespace Core.Game
{
    public class GamePhaseEntryExecutor : PhaseExecutor<GamePhase, IGamePhaseEntryHandler>, IGamePhaseEntryExecutor
    {
        public override GamePhase Phase => GamePhase.Entry;
    }
}