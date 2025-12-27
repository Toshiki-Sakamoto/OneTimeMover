using Core.Stage;

namespace OneTripMover.Infrastructure
{
    public class StageRepository : IStageRepository
    {
        private StageId _currentStageId;
        
        public void SetCurrentState(StageId stageId)
        {
            _currentStageId = stageId;
        }

        public StageId GetCurrentStageId()
        {
            return _currentStageId;
        }
    }
}