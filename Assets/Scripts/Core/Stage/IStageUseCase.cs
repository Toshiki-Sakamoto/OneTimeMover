using Core.Cargo;

namespace Core.Stage
{
    public interface IStageUseCase
    {
        /// <summary>
        /// 現在のステージ
        /// </summary>
        void SetCurrentStage(StageId stageId);
        
        StageId GetCurrentStage();

        /// <summary>
        /// ボーナス状態
        /// </summary>
        bool HasPerfectBonus();
        bool HasOneMoreBonus();
        void SetPerfectBonus(bool enabled);
        void SetOneMoreBonus(bool enabled);
        void GainOneMoreBonus();
        void LosePerfectBonus();
        void LoseOneMoreBonus();
        
        /// <summary>
        /// 初期荷物数の取得
        /// </summary>
        int GetInitCargoNum();
        
        /// <summary>
        /// 現在のステージからランダムに荷物を取得する
        /// </summary>
        ICargoMaster GetRandomCargoMaster();
        
        /// <summary>
        /// 現在のステージからランダムにドロップ用荷物を取得する
        /// </summary>
        ICargoMaster GetRandomDropCargoMaster();
    }
}
