using System.Collections.Generic;
using Core.Cargo;
using OneTripMover.Core;
using UnityEngine;

namespace OneTripMover.Master
{
    public class CargoMasterRegistry : MasterDataRegistry<ICargoMaster>, ICargoMasterRegistry
    {
    }
}