using Core.Cargo;
using OneTripMover.Master;

namespace Core.Stage
{
    public interface IStageMaster : IMasterData<IStageMaster>
    {
        int StageId { get; }
        
        string Scene { get; }
        
        int InitCargoNum { get; }
        
        /// <summary>
        /// 初期積荷マスター一覧
        /// </summary>
        ICargoMaster[] InitCargoMasters { get; }
        
        /// <summary>
        /// ドロップ積荷マスター一覧
        /// </summary>
        ICargoMaster[] DropCargoMasters { get; }

        Core.Adventure.AdvText IntroAdventure { get; }
        Core.Adventure.AdvText ClearAdventure { get; }
    }
}
