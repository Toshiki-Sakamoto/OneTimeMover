using OneTripMover.Core;
using OneTripMover.Core.Entity;

namespace Core.Cargo
{
    public interface ICargo : IEntity
    {
        CargoId CargoId { get; }
    }
}