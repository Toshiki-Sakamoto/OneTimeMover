using Core.Common;
using Core.Player;
using Core.Phase;
using Core.Stage;

namespace Core.Game
{
    public class GamePhaseEntryHandler : IGamePhaseEntryHandler
    {
        private readonly IStageUseCase _stageUseCase;
        private readonly IPlayerStatusUseCase _playerStatusUseCase;
        
        public GamePhaseEntryHandler()
        {
            _stageUseCase = ServiceLocator.Resolve<IStageUseCase>();
            _playerStatusUseCase = ServiceLocator.Resolve<IPlayerStatusUseCase>();
        }
        
        public void OnEnter(IPhaseContext<GamePhase> context)
        {
            _playerStatusUseCase.InitializeFromMaster();
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
