using System.Collections.Generic;
using Core.Common;
using OneTripMover.Core;
using OneTripMover.Master;

namespace Core.Cargo
{
    public class CargoRegistry : ICargoRegistry
    {
        private readonly Dictionary<CargoId, ICargo> _cargos = new();
        private ICargo _topCargo;

        public void Register(ICargo cargo)
        {
            _topCargo = cargo;
            _cargos[cargo.CargoId] = cargo;
        }

        public void Unregister(ICargo cargo)
        {
            _cargos.Remove(cargo.CargoId);
            if (_topCargo == cargo)
            {
                _topCargo = null;
            }
        }

        public void Clear()
        {
            _cargos.Clear();
            _topCargo = null;
        }

        public ICargo GetTopCargo() => _topCargo;

        public bool TryGet(CargoId cargoId, out ICargo cargo) => _cargos.TryGetValue(cargoId, out cargo);
    }
}
