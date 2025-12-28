using Core.Common;
using OneTripMover.Core;
using OneTripMover.Core.Entity;

namespace Core.Cargo
{
    public class CargoFactory : ICargoFactory
    {
        private readonly ICargoUseCase _cargoUseCase;
        private IEntityForIdFactory<Cargo, Cargo> _cargoEntityFactory;
        
        public CargoFactory()
        {
            _cargoUseCase = ServiceLocator.Resolve<ICargoUseCase>();
            _cargoEntityFactory = ServiceLocator.Resolve<IEntityForIdFactory<Cargo, Cargo>>();
        }
        
        public ICargo Create(ICargoMaster master, bool isOneMoreBonus = false)
        {
            var cargo = _cargoEntityFactory.Create();
            cargo.IsOneMoreBonus = isOneMoreBonus;
            cargo.MasterId = master.Id;
            _cargoUseCase.AddCargo(cargo, isOneMoreBonus);

            return cargo;
        }
    }
}
