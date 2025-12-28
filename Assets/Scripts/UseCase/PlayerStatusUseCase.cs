using Core.Common;
using Core.Player;
using OneTripMover.Master;

namespace OneTripMover.UseCase
{
    public class PlayerStatusUseCase : IPlayerStatusUseCase
    {
        private readonly IPlayerStatusRepository _repository;
        private readonly IPlayerMasterRegistry _masterRegistry;

        public PlayerStatusUseCase()
        {
            _repository = ServiceLocator.Resolve<IPlayerStatusRepository>();
            _masterRegistry = ServiceLocator.Resolve<IPlayerMasterRegistry>();
        }

        public void SetLimitAngle(float angleDeg)
        {
            _repository.SetLimitAngle(angleDeg);
        }

        public void AddLimitAngle(float deltaDeg)
        {
            _repository.AddLimitAngle(deltaDeg);
        }

        public float GetLimitAngle()
        {
            var current = _repository.GetLimitAngle();
            if (current <= 0f)
            {
                InitializeFromMaster();
                current = _repository.GetLimitAngle();
            }
            return current;
        }

        public void InitializeFromMaster()
        {
            var playerMaster = _masterRegistry.GetMaster();
            var initial = playerMaster?.LimitAngleDeg ?? 45f;
            _repository.SetLimitAngle(initial);
        }
    }
}
