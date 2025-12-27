using System.Collections.Generic;
using Core.Stage;

namespace OneTripMover.Master
{
    public interface IStageMasterRegistry : IMasterDataRegistry<IStageMaster>
    {
        bool TryGetByStageId(int stageId, out IStageMaster stageMaster);
    }
}
