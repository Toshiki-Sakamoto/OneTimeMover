using Core.Cargo;
using OneTripMover.Master;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Core.Stage
{
    public interface IStageMaster : IMasterData<IStageMaster>
    {
        int StageId { get; }
        
        string Scene { get; }
        
        int InitCargoNum { get; }
        
        /// <summary>
        /// 初期積荷マスター一覧を取得
        /// </summary>
        ICargoMaster[] InitCargoMasters { get; }
    }
}
