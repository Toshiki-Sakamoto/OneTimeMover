namespace Core.Stage
{
    public interface IStageRepository
    {
        void SetCurrentState(StageId stageId);
        StageId GetCurrentStageId();
    }
}