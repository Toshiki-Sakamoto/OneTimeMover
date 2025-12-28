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

        public bool TryGetValue(MasterId<IPlayerMaster> id, out IPlayerMaster master)
        {
            // 単一マスター想定のため、存在すれば常に返す
            master = _master;
            return master != null;
        }
    }
}
