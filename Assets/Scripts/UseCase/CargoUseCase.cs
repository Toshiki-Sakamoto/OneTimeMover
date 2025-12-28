using Core.Cargo;
using Core.Common;
using Core.Common.Messaging;
using Core.Game;
using Core.Money;
using Core.Phase;
using OneTripMover.Core;
using OneTripMover.Core.Entity;
using OneTripMover.Master;
using UnityEngine;

namespace OneTripMover.UseCase
{
    public class CargoUseCase : ICargoUseCase
    {
        private ICargoRegistry _cargoRegistry;
        private IPhaseService<GamePhase> _gamePhaseService;
        private IPublisher<CargoAddedEvent> _cargoAddedEventPublisher;
        private ICargoMasterRegistry _cargoMasterRegistry;
        private IMoneyUseCase _moneyUseCase;
        private IPublisher<MoneyTransactionEvent> _moneyTransactionPublisher;
        
        public CargoUseCase()
        {
            _cargoRegistry = ServiceLocator.Resolve<ICargoRegistry>();
            _cargoAddedEventPublisher = ServiceLocator.Resolve<IPublisher<CargoAddedEvent>>();
            _gamePhaseService = ServiceLocator.Resolve<IPhaseService<GamePhase>>();
            _cargoMasterRegistry = ServiceLocator.Resolve<ICargoMasterRegistry>();
            _moneyUseCase = ServiceLocator.Resolve<IMoneyUseCase>();
            _moneyTransactionPublisher = ServiceLocator.Resolve<IPublisher<MoneyTransactionEvent>>();
        }
        
        public void AddCargo(ICargo cargo, bool isOneMore)
        {
            _cargoRegistry.Register(cargo);
            
            _cargoAddedEventPublisher.Publish(new CargoAddedEvent { Cargo = cargo, IsOneMoreCargo = isOneMore });
        }

        public void RemoveCargo(IEntityId cargoId)
        {
            if (!_cargoRegistry.TryGet(cargoId, out var cargo)) return;

            _cargoRegistry.Unregister(cargo);

            if (_cargoMasterRegistry.TryGetValue(cargo.MasterId, out var cargoMaster))
            {
                var cost = cargoMaster.Cost.Amount;
                if (cargo.IsOneMoreBonus)
                {
                    cost *= 2;
                }
                if (cost > 0)
                {
                    _moneyUseCase.SubtractMoney(cost);
                    _moneyTransactionPublisher?.Publish(new MoneyTransactionEvent
                    {
                        Delta = -cost,
                        Current = _moneyUseCase.GetMoney(),
                        IsPenalty = true
                    });
                }
            }
        }
    }
}
