using Core.Common;
using Core.Common.Messaging;
using Core.Cargo;
using Views.Cargo;

namespace OneTripMover.UseCase
{
    public class CargoEventHandler : ICargoEventHandler
    {
        private ICargoRegistry _cargoRegistry;

        [Inject]
        public void Construct()
        {
            _cargoRegistry = ServiceLocator.Resolve<ICargoRegistry>();
            var detachedSubscriber = ServiceLocator.Resolve<ISubscriber<CargoViewDetachedEvent>>();
            detachedSubscriber.Subscribe(OnCargoDetached);
        }

        private void OnCargoDetached(CargoViewDetachedEvent evt)
        {
            if (evt == null) return;
            if (!_cargoRegistry.TryGet(evt.CargoId, out var cargo)) return;
            _cargoRegistry.Unregister(cargo);
        }
    }
}
