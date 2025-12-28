using OneTripMover.Core;
using OneTripMover.Master;
using UnityEngine.AddressableAssets;

namespace Core.Cargo
{
    public interface ICargoMaster : IMasterData<ICargoMaster>
    {
        public AssetReferenceGameObject CargoView { get; }
        public Money.Money Cost { get; }
    }
}