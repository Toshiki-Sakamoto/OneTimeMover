using OneTripMover.Core;
using UnityEngine;

namespace Views.Cargo
{
    public class CargoViewGroundHitEvent
    {
        public CargoId CargoId;
        public CargoView CargoView;
        public Collision2D Collision;
    }
}
