using OneTripMover.Master;

namespace Core.Player
{
    public interface IPlayerMaster : IMasterData<IPlayerMaster>
    {
        float LimitAngleDeg { get; }
    }
}
