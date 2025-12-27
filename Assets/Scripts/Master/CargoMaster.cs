using System;
using Core.Cargo;
using Core.Money;
using OneTripMover.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Views.Cargo;

namespace OneTripMover.Master
{
    [CreateAssetMenu(fileName = "CargoMaster", menuName = "OneTripMover/MasterData/CargoMaster")]
    public class CargoMaster : AddressableMasterData<ICargoMaster>, ICargoMaster
    {
        [SerializeField] private AssetReferenceGameObject _cargoView;
        [SerializeField] private CargoId _cargoId;
        [SerializeField] private Money _cost;
        
        public AssetReferenceGameObject CargoView => _cargoView;
        public CargoId CargoId => _cargoId;
        public Money Cost => _cost;
    }
}