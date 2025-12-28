using Core.Common;
using Core.Player;
using OneTripMover.Master;

namespace OneTripMover.UseCase
{
    public class PlayerStatusUseCase : IPlayerStatusUseCase
    {
        private readonly IPlayerStatusRepository _repository;

        public PlayerStatusUseCase()
        {
            _repository = ServiceLocator.Resolve<IPlayerStatusRepository>();

            // 初期値はPlayerMasterから取得できるものがあればそれを設定
            var playerMaster = ServiceLocator.Resolve<IPlayerMasterRegistry>().GetMaster();
            if (playerMaster != null)
            {
                _repository.SetLimitAngle(playerMaster.LimitAngleDeg);
            }
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
            return _repository.GetLimitAngle();
        }
    }
}
