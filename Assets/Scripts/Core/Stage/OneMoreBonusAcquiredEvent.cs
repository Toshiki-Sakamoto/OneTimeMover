using Core.Cargo;
using OneTripMover.Core;
using OneTripMover.Core.Entity;
using OneTripMover.Master;

namespace Core.Stage
{
    public class OneMoreBonusAcquiredEvent
    {
        public MasterId<ICargoMaster> CargoMasterId;
    }
}
