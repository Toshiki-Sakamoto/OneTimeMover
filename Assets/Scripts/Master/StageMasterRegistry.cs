using System.Collections.Generic;
using Core.Stage;
using Master;

namespace OneTripMover.Master
{
    public class StageMasterRegistry : MasterDataRegistry<IStageMaster>, IStageMasterRegistry
    {
        private readonly Dictionary<int, IStageMaster> _stageIdToMasters = new();

        protected override void RegisterCore(IStageMaster master)
        {
            _stageIdToMasters[master.StageId] = master;
        }
        
        public bool TryGetByStageId(int stageId, out IStageMaster stageMaster)
        {
            return _stageIdToMasters.TryGetValue(stageId, out stageMaster);
        }
    }
}
