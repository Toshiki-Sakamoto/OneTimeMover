using Core.Common;
using Core.Phase;
using Core.Stage;

namespace Core.Game
{
    public class GamePhaseEntryHandler : IGamePhaseEntryHandler
    {
        private readonly IStageUseCase _stageUseCase;
        
        public GamePhaseEntryHandler()
        {
            _stageUseCase = ServiceLocator.Resolve<IStageUseCase>();
        }
        
        public void OnEnter(IPhaseContext<GamePhase> context)
        {
            _stageUseCase.SetCurrentStage(1);
            
            context.RequestChange(GamePhase.Initialize);
        }

        public void OnUpdate(IPhaseContext<GamePhase> context, float deltaTime)
        {
        }

        public void OnExit(IPhaseContext<GamePhase> context)
        {
        }
    }
}
