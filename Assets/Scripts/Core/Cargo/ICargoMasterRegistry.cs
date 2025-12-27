using System.Collections.Generic;
using Core.Cargo;
using OneTripMover.Core;
using OneTripMover.Master;

namespace OneTripMover.Master
{
    public interface ICargoMasterRegistry : IMasterDataRegistry<ICargoMaster>
    {
        bool TryGetByCargoId(CargoId cargoId, out ICargoMaster cargoMaster);
        
        IEnumerable<ICargoMaster> GetAllCargoMasters();
    }
}