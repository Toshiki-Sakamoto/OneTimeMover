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
        /// 初期荷物セット
        /// </summary>
        ICargoMaster[] InitCargoMasters { get; }
        
        /// <summary>
        /// ドロップ荷物セット
        /// </summary>
        ICargoMaster[] DropCargoMasters { get; }

        int PerfectBonusAmount { get; }
        int OneMoreBonusAmount { get; }

        Core.Adventure.AdvText IntroAdventure { get; }
        Core.Adventure.AdvText ClearAdventure { get; }
    }
}
