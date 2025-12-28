using Core.Common;
using Core.Common.Messaging;
using Core.Cargo;
using Views.Cargo;

namespace OneTripMover.UseCase
{
    public class CargoEventHandler : ICargoEventHandler
    {
        private ICargoUseCase _cargoUseCase;

        [Inject]
        public void Construct()
        {
            _cargoUseCase = ServiceLocator.Resolve<ICargoUseCase>();
            var detachedSubscriber = ServiceLocator.Resolve<ISubscriber<CargoViewDetachedEvent>>();
            detachedSubscriber.Subscribe(OnCargoDetached);
        }

        private void OnCargoDetached(CargoViewDetachedEvent evt)
        {
            if (evt == null) return;
            _cargoUseCase.RemoveCargo(evt.CargoId);
        }
    }
}
