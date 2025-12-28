using Core;
using Core.Cargo;
using Core.Common;
using Core.Common.Messaging;
using Core.Game;
using Core.Stage;
using OneTripMover.Master;

namespace OneTripMover.UseCase
{
    public class StageEventHandler : IStageEventHandler, IEventHandler
    {
        private IStageUseCase _stageUseCase;
        private ICargoMasterRegistry _cargoMasterRegistry;
        private ICargoFactory _cargoFactory;
        
        public StageEventHandler()
        {
            _stageUseCase = ServiceLocator.Resolve<IStageUseCase>();
            _cargoMasterRegistry = ServiceLocator.Resolve<ICargoMasterRegistry>();
            _cargoFactory = ServiceLocator.Resolve<ICargoFactory>();
            
            var playerStatePrepareStartedEvent = ServiceLocator.Resolve<ISubscriber<PlayerStatePrepareStartedEvent>>();
            playerStatePrepareStartedEvent.Subscribe(OnPlayerStatePrepareStarted);

            var oneMoreAcquiredSub = ServiceLocator.Resolve<ISubscriber<OneMoreBonusAcquiredEvent>>();
            oneMoreAcquiredSub.Subscribe(OnOneMoreBonusAcquired);
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

        private void OnOneMoreBonusAcquired(OneMoreBonusAcquiredEvent evt)
        {
            // ワンモア獲得時に追加荷物を生成
            _cargoMasterRegistry.TryGetValue(evt.CargoMasterId, out var master);
            _cargoFactory.Create(master, isOneMoreBonus: true);
        }
    }
}
