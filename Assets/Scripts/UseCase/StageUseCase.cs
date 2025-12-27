using Core.Cargo;
using Core.Common;
using Core.Stage;
using OneTripMover.Master;

namespace OneTripMover.UseCase
{
    public class StageUseCase : IStageUseCase
    {
        private readonly IStageRepository _stageRepository;
        private readonly IStageMasterRegistry _stageMasterRegistry;
        
        public StageUseCase()
        {
            _stageRepository = ServiceLocator.Resolve<IStageRepository>();
            _stageMasterRegistry = ServiceLocator.Resolve<IStageMasterRegistry>();
        }
        
        public void SetCurrentStage(int stageId)
        {
            _stageRepository.SetCurrentState(stageId);
        }

        public void SetCurrentStage(StageId stageId)
        {
            _stageRepository.SetCurrentState(stageId);
        }

        public StageId GetCurrentStage()
        {
            return _stageRepository.GetCurrentStageId();
        }

        public int GetInitCargoNum()
        {
            return GetCurrentStageMaster().InitCargoNum;
        }

        public ICargoMaster GetRandomCargoMaster()
        {
            var stageMaster = GetCurrentStageMaster();
            var randomIndex = UnityEngine.Random.Range(0, stageMaster.InitCargoMasters.Length);
            return stageMaster.InitCargoMasters[randomIndex];
        }

        private IStageMaster GetCurrentStageMaster()
        {
            var currentStageId = _stageRepository.GetCurrentStageId();
            return _stageMasterRegistry.TryGetByStageId(currentStageId, out IStageMaster stageMaster) ? stageMaster : null;
        }
    }
}
