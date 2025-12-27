using System.Threading.Tasks;
using Core.Cargo;
using Core.Common;
using Core.Common.Messaging;
using Core.Phase;
using Core.Stage;
using UnityEngine;

namespace Core.Game
{
    public class GamePhaseInitializeHandler : IGamePhaseInitializeHandler
    {
        private readonly IStageUseCase _stageUseCase;
        private readonly ICargoUseCase _cargoUseCase;
        private readonly ICargoFactory _cargoFactory;
        
        public GamePhaseInitializeHandler()
        {
            _stageUseCase = ServiceLocator.Resolve<IStageUseCase>();
            _cargoUseCase = ServiceLocator.Resolve<ICargoUseCase>();
            _cargoFactory = ServiceLocator.Resolve<ICargoFactory>();
        }
        

        public void OnEnter(IPhaseContext<GamePhase> context)
        {
//            var initNum = _stageUseCase.GetInitCargoNum();
//            for (var i = 0; i < initNum; ++i)
//            {
//                var cargo = _cargoFactory.Create(_stageUseCase.GetRandomCargoMaster());
//            }

//            DelayStartAsync(context);
        }

        public void OnUpdate(IPhaseContext<GamePhase> context, float deltaTime)
        {
        }

        public void OnExit(IPhaseContext<GamePhase> context)
        {
        }
        
        private async Task DelayStartAsync(IPhaseContext<GamePhase> context)
        {
            await Task.Delay(1000);
            
            context.RequestChange(GamePhase.Play);
        }
    }
}
