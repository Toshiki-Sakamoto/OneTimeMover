using Core.Common;
using OneTripMover.Core;
using OneTripMover.Core.Entity;
using OneTripMover.Master;
using UnityEngine;

namespace Core.Cargo
{
    /// <summary>
    /// 荷物を表す構造体
    /// </summary>
    public class Cargo : ICargo
    {
        public IEntityId Id { get; private set; }
        
        public MasterId<ICargoMaster> MasterId { get; set; }

        public bool IsOneMoreBonus { get; set; }

        public Cargo()
        {
        }

        public void AssignId(IEntityId id) =>
            Id = id;
    }
}
