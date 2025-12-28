using OneTripMover.Master;

namespace Core.Player
{
    public interface IPlayerMaster : IMasterData<IPlayerMaster>
    {
        float LimitAngleDeg { get; }
        int InitialMoney { get; }
        float MoveForce { get; }
        float MaxMoveSpeed { get; }
    }
}
