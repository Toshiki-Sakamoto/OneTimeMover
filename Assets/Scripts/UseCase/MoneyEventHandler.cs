using Core.Common;
using Core.Common.Messaging;
using Core.Money;
using OneTripMover.Master;
using Views.Cargo;

namespace OneTripMover.UseCase
{
    public class MoneyEventHandler : IMoneyEventHandler
    {
        private IMoneyUseCase _moneyUseCase;
        private ICargoMasterRegistry _cargoMasterRegistry;

        public MoneyEventHandler()
        {
            _moneyUseCase = ServiceLocator.Resolve<IMoneyUseCase>();
            _cargoMasterRegistry = ServiceLocator.Resolve<ICargoMasterRegistry>();

            var groundHitSubscriber = ServiceLocator.Resolve<ISubscriber<CargoViewGroundHitEvent>>();
            groundHitSubscriber.Subscribe(OnCargoGroundHit);
        }

        private void OnCargoGroundHit(CargoViewGroundHitEvent evt)
        {
            if (!_cargoMasterRegistry.TryGetByCargoId(evt.CargoId, out var cargoMaster)) return;

            var cost = cargoMaster.Cost.Amount;
            if (cost <= 0) return;

            _moneyUseCase.SubtractMoney(cost);
        }
    }
}
