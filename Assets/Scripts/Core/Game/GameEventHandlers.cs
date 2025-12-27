using Core.Common;
using OneTripMover.UseCase;

namespace Core.Game
{
    public class GameEventHandlers : IGameEventHandlers
    {
        private IStageEventHandler _stageEventHandler;
        
        public GameEventHandlers()
        {
            _stageEventHandler = ServiceLocator.Resolve<IStageEventHandler>();
        }
    }
}