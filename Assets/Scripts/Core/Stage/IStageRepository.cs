namespace Core.Stage
{
    public interface IStageRepository
    {
        void SetCurrentState(int stageId);
        int GetCurrentStageId();
    }
}