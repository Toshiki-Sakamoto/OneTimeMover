using Core.Common;
using OneTripMover.Core;
using OneTripMover.Core.Entity;
using UnityEngine;

namespace Core.Cargo
{
    /// <summary>
    /// 荷物を表す構造体
    /// </summary>
    public class Cargo : ICargo
    {
        public IEntityId Id { get; private set; }
        
        public CargoId CargoId { get; private set; }


        public Cargo(CargoId cargoId) 
        {
            CargoId = cargoId;
        }
        
        public void AssignId(IEntityId id) =>
            Id = id;
    }
}