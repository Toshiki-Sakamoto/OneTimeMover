using Core.Common;
using Core.Phase;
using Core.Stage;

namespace Core.Game
{
    public class GamePhaseEntryHandler : IGamePhaseEntryHandler
    {
        public void OnEnter(IPhaseContext<GamePhase> context)
        {
            var stageUseCase = ServiceLocator.Resolve<IStageUseCase>();
            stageUseCase.SetCurrentStage(1);
            
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
