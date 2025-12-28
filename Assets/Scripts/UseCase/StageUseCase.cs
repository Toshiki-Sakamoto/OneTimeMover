using Core.Cargo;
using Core.Common;
using Core.Common.Messaging;
using Core.Stage;
using OneTripMover.Master;
using UnityEngine;

namespace OneTripMover.UseCase
{
    public class StageUseCase : IStageUseCase
    {
        private readonly IStageRepository _stageRepository;
        private readonly IStageMasterRegistry _stageMasterRegistry;
        private readonly IPublisher<PerfectBonusChangedEvent> _perfectBonusPublisher;
        private readonly IPublisher<OneMoreBonusChangedEvent> _oneMoreBonusPublisher;
        
        public StageUseCase()
        {
            _stageRepository = ServiceLocator.Resolve<IStageRepository>();
            _stageMasterRegistry = ServiceLocator.Resolve<IStageMasterRegistry>();
            _perfectBonusPublisher = ServiceLocator.Resolve<IPublisher<PerfectBonusChangedEvent>>();
            _oneMoreBonusPublisher = ServiceLocator.Resolve<IPublisher<OneMoreBonusChangedEvent>>();
        }
        
        public void SetCurrentStage(int stageId)
        {
            SetCurrentStage(new StageId(stageId));
        }

        public void SetCurrentStage(StageId stageId)
        {
            _stageRepository.SetCurrentState(stageId);
            SetPerfectBonusInternal(true, publish: true, forcePublish: true);
            SetOneMoreBonusInternal(false, publish: true, forcePublish: true);
        }

        public StageId GetCurrentStage()
        {
            return _stageRepository.GetCurrentStageId();
        }

        public bool HasPerfectBonus()
        {
            return _stageRepository.GetPerfectBonus();
        }

        public bool HasOneMoreBonus()
        {
            return _stageRepository.GetOneMoreBonus();
        }

        public void SetPerfectBonus(bool enabled)
        {
            SetPerfectBonusInternal(enabled, publish: true);
        }

        public void SetOneMoreBonus(bool enabled)
        {
            SetOneMoreBonusInternal(enabled, publish: true);
        }

        public void GainOneMoreBonus()
        {
            SetOneMoreBonusInternal(true, publish: true);
        }

        public void LosePerfectBonus()
        {
            SetPerfectBonusInternal(false, publish: true);
        }

        public void LoseOneMoreBonus()
        {
            SetOneMoreBonusInternal(false, publish: true);
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

        public ICargoMaster GetRandomDropCargoMaster()
        {
            var stageMaster = GetCurrentStageMaster();
            var masters = stageMaster?.DropCargoMasters;
            if (masters == null || masters.Length == 0) return null;
            var randomIndex = UnityEngine.Random.Range(0, masters.Length);
            return masters[randomIndex];
        }

        private IStageMaster GetCurrentStageMaster()
        {
            var currentStageId = _stageRepository.GetCurrentStageId();
            return _stageMasterRegistry.TryGetByStageId(currentStageId, out IStageMaster stageMaster) ? stageMaster : null;
        }

        private void SetPerfectBonusInternal(bool enabled, bool publish, bool forcePublish = false)
        {
            if (!forcePublish && _stageRepository.GetPerfectBonus() == enabled) return;
            _stageRepository.SetPerfectBonus(enabled);
            if (publish)
            {
                _perfectBonusPublisher?.Publish(new PerfectBonusChangedEvent { Enabled = enabled });
            }
        }

        private void SetOneMoreBonusInternal(bool enabled, bool publish, bool forcePublish = false)
        {
            if (!forcePublish && _stageRepository.GetOneMoreBonus() == enabled) return;
          
            _stageRepository.SetOneMoreBonus(enabled);
            if (publish)
            {
                _oneMoreBonusPublisher?.Publish(new OneMoreBonusChangedEvent { Enabled = enabled });
            }
        }
    }
}
