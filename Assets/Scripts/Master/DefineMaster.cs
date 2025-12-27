using Core;
using Core.Cargo;
using OneTripMover.Master;
using UnityEngine;

namespace OneTripMover.Master
{
    [CreateAssetMenu(fileName = "DefineMaster", menuName = "OneTripMover/Master/DefineMaster")]
    public class DefineMaster : AddressableMasterData<IDefineMaster>, IDefineMaster
    {
    }
}