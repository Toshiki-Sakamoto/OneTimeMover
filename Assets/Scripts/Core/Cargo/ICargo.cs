using OneTripMover.Core;
using OneTripMover.Core.Entity;
using OneTripMover.Master;

namespace Core.Cargo
{
    public interface ICargo : IEntity
    {
        MasterId<ICargoMaster> MasterId { get; }
        bool IsOneMoreBonus { get; }
    }
}
