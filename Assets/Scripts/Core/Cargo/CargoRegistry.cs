using System.Collections.Generic;
using Core.Common;
using OneTripMover.Core;
using OneTripMover.Core.Entity;
using OneTripMover.Master;

namespace Core.Cargo
{
    public class CargoRegistry : ICargoRegistry
    {
        private readonly Dictionary<IEntityId, ICargo> _cargos = new();
        private ICargo _topCargo;

        public void Register(ICargo cargo)
        {
            _topCargo = cargo;
            _cargos[cargo.Id] = cargo;
        }

        public void Unregister(ICargo cargo)
        {
            _cargos.Remove(cargo.Id);
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

        public bool TryGet(IEntityId cargoId, out ICargo cargo) => _cargos.TryGetValue(cargoId, out cargo);
    }
}
