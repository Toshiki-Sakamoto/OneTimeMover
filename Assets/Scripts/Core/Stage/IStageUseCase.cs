using Core.Cargo;

namespace Core.Stage
{
    public interface IStageUseCase
    {
        /// <summary>
        /// 現在のステージ
        /// </summary>
        void SetCurrentStage(int stageId);
        
        /// <summary>
        /// 初期荷物数の取得
        /// </summary>
        int GetInitCargoNum();
        
        /// <summary>
        /// 現在のステージからランダムに荷物を取得する
        /// </summary>
        ICargoMaster GetRandomCargoMaster();
    }
}