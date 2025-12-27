using Core.Cargo;
using Core.Common;
using Core.Common.Messaging;
using Core.Game;
using Core.Phase;
using OneTripMover.Core;
using UnityEngine;

namespace OneTripMover.UseCase
{
    public class CargoUseCase : ICargoUseCase
    {
        private ICargoRegistry _cargoRegistry;
        private IPhaseService<GamePhase> _gamePhaseService;
        private IPublisher<CargoAddedEvent> _cargoAddedEventPublisher;
        
        public CargoUseCase()
        {
            _cargoRegistry = ServiceLocator.Resolve<ICargoRegistry>();
            _cargoAddedEventPublisher = ServiceLocator.Resolve<IPublisher<CargoAddedEvent>>();
            _gamePhaseService = ServiceLocator.Resolve<IPhaseService<GamePhase>>();
        }
        
        public void AddCargo(ICargo cargo)
        {
            _cargoRegistry.Register(cargo);
            
            // ゲーム開始前はBreakは無視
            _cargoAddedEventPublisher.Publish(new CargoAddedEvent { Cargo = cargo });
        }
    }
}