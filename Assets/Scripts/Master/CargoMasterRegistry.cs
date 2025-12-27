using System.Collections.Generic;
using Core.Cargo;
using OneTripMover.Core;
using UnityEngine;

namespace OneTripMover.Master
{
    public class CargoMasterRegistry : MasterDataRegistry<ICargoMaster>, ICargoMasterRegistry
    {
        private readonly Dictionary<CargoId, ICargoMaster> _cargoIdToMasters = new();

        protected override void RegisterCore(ICargoMaster master)
        {
            _cargoIdToMasters[master.CargoId] = master;
        }

        protected override void ClearCore()
        {
            _cargoIdToMasters.Clear();
        }

        public bool TryGetByCargoId(CargoId cargoId, out ICargoMaster cargoMaster)
        {
            return _cargoIdToMasters.TryGetValue(cargoId, out cargoMaster);
        }

        public IEnumerable<ICargoMaster> GetAllCargoMasters()
        {
            return _cargoIdToMasters.Values;
        }
    }
}