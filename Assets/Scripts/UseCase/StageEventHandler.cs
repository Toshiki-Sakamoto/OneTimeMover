using Core;
using Core.Cargo;
using Core.Common;
using Core.Common.Messaging;
using Core.Game;
using Core.Stage;

namespace OneTripMover.UseCase
{
    public class StageEventHandler : IStageEventHandler, IEventHandler
    {
        private IStageUseCase _stageUseCase;
        private ICargoUseCase _cargoUseCase;
        private ICargoFactory _cargoFactory;
        
        [Inject]
        public void Construct()
        {
            _stageUseCase = ServiceLocator.Resolve<IStageUseCase>();
            _cargoUseCase = ServiceLocator.Resolve<ICargoUseCase>();
            _cargoFactory = ServiceLocator.Resolve<ICargoFactory>();
            
            var playerStatePrepareStartedEvent = ServiceLocator.Resolve<ISubscriber<PlayerStatePrepareStartedEvent>>();
            playerStatePrepareStartedEvent.Subscribe(OnPlayerStatePrepareStarted);
        }
        
        /// <summary>
        /// プレイヤーの開始準備を行う
        /// </summary>
        private void OnPlayerStatePrepareStarted(PlayerStatePrepareStartedEvent evt)
        {
            for (var i = 0; i < _stageUseCase.GetInitCargoNum(); ++i)
            {
                _cargoFactory.Create(_stageUseCase.GetRandomCargoMaster());
            }
        }
    }
}