using Core;
using Core.Common;
using Core.Common.Messaging;
using Core.Cargo;
using Core.Stage;
using OneTripMover.Core.Entity;
using Views.Cargo;

namespace OneTripMover.UseCase
{
    public class BonusStatusEventHandler : IEventHandler
    {
        private readonly IStageUseCase _stageUseCase;
        private readonly ICargoRegistry _cargoRegistry;

        public BonusStatusEventHandler()
        {
            _stageUseCase = ServiceLocator.Resolve<IStageUseCase>();
            _cargoRegistry = ServiceLocator.Resolve<ICargoRegistry>();

            var groundHitSub = ServiceLocator.Resolve<ISubscriber<CargoViewGroundHitEvent>>();
            groundHitSub.Subscribe(OnCargoGroundHit);

            var oneMoreSub = ServiceLocator.Resolve<ISubscriber<OneMoreBonusAcquiredEvent>>();
            oneMoreSub.Subscribe(OnOneMoreAcquired);

            var detachedSub = ServiceLocator.Resolve<ISubscriber<CargoViewDetachedEvent>>();
            detachedSub.Subscribe(OnCargoDetached);
        }

        private void OnCargoGroundHit(CargoViewGroundHitEvent evt)
        {
            _stageUseCase.LosePerfectBonus();
            if (IsOneMoreCargo(evt.CargoId))
            {
                _stageUseCase.LoseOneMoreBonus();
            }
        }

        private void OnCargoDetached(CargoViewDetachedEvent evt)
        {
            _stageUseCase.LosePerfectBonus();
            if (IsOneMoreCargo(evt.CargoId))
            {
                _stageUseCase.LoseOneMoreBonus();
            }
        }

        private void OnOneMoreAcquired(OneMoreBonusAcquiredEvent evt)
        {
            _stageUseCase.GainOneMoreBonus();
        }

        private bool IsOneMoreCargo(IEntityId cargoId)
        {
            if (_cargoRegistry != null && _cargoRegistry.TryGet(cargoId, out var cargo))
            {
                return cargo.IsOneMoreBonus;
            }
            return false;
        }
    }
}
