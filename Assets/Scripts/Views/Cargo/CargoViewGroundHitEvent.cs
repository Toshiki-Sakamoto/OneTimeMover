using OneTripMover.Core;
using OneTripMover.Core.Entity;
using UnityEngine;

namespace Views.Cargo
{
    public class CargoViewGroundHitEvent
    {
        public IEntityId CargoId;
        public CargoView CargoView;
        public Collision2D Collision;
    }
}
