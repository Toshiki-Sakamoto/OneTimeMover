using System.Collections.Generic;
using Core.Player;
using OneTripMover.Master;

namespace OneTripMover.Master
{
    public class PlayerMasterRegistry : IMasterDataRegistry<IPlayerMaster>, IPlayerMasterRegistry
    {
        private IPlayerMaster _master;

        public void Register(IPlayerMaster master)
        {
            _master = master;
        }

        public void Clear()
        {
            _master = null;
        }

        public IPlayerMaster GetMaster() => _master;
    }
}
