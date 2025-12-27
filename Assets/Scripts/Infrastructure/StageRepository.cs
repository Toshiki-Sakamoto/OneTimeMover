using Core.Stage;

namespace OneTripMover.Infrastructure
{
    public class StageRepository : IStageRepository
    {
        private int _currentStageId;
        
        public void SetCurrentState(int stageId)
        {
            _currentStageId = stageId;
        }

        public int GetCurrentStageId()
        {
            return _currentStageId;
        }
    }
}