namespace Core.Stage
{
    public interface IStageRepository
    {
        void SetCurrentState(StageId stageId);
        StageId GetCurrentStageId();

        void SetPerfectBonus(bool enabled);
        bool GetPerfectBonus();

        void SetOneMoreBonus(bool enabled);
        bool GetOneMoreBonus();
    }
}
