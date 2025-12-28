using Core.Stage;

namespace OneTripMover.Infrastructure
{
    public class StageRepository : IStageRepository
    {
        private StageId _currentStageId;
        private bool _perfectBonus = true;
        private bool _oneMoreBonus;
        
        public void SetCurrentState(StageId stageId)
        {
            _currentStageId = stageId;
        }

        public StageId GetCurrentStageId()
        {
            return _currentStageId;
        }

        public void SetPerfectBonus(bool enabled)
        {
            _perfectBonus = enabled;
        }

        public bool GetPerfectBonus()
        {
            return _perfectBonus;
        }

        public void SetOneMoreBonus(bool enabled)
        {
            _oneMoreBonus = enabled;
        }

        public bool GetOneMoreBonus()
        {
            return _oneMoreBonus;
        }
    }
}
