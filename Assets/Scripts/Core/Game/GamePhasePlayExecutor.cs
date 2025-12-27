using Core.Phase;

namespace Core.Game
{
    public class GameGamePhasePlayExecutor : PhaseExecutor<GamePhase, IGamePhasePlayHandler>, IGamePhasePlayExecutor
    {
        public override GamePhase Phase => GamePhase.Play;
    }
}