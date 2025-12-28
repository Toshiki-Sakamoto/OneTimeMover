using OneTripMover.Core;
using OneTripMover.Core.Entity;

namespace Core.Cargo
{
    public interface ICargoUseCase
    {
        void AddCargo(ICargo cargo, bool isOneMore);
        void RemoveCargo(IEntityId cargoId);
    }
}
