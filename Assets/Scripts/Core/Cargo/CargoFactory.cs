using Core.Common;

namespace Core.Cargo
{
    public class CargoFactory : ICargoFactory
    {
        private readonly ICargoUseCase _cargoUseCase;
        
        public CargoFactory()
        {
            _cargoUseCase = ServiceLocator.Resolve<ICargoUseCase>();
        }
        
        public ICargo Create(ICargoMaster master)
        {
            var cargo = new Cargo(master.CargoId);
            _cargoUseCase.AddCargo(cargo);

            return cargo;
        }
    }
}