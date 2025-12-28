using OneTripMover.Master;

namespace Core.Player
{
    public interface IPlayerMasterRegistry : IMasterDataRegistry<IPlayerMaster>
    {
        IPlayerMaster GetMaster();
    }
}
