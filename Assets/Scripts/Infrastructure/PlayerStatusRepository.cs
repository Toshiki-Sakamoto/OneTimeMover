using Core.Player;

namespace OneTripMover.Infrastructure
{
    public class PlayerStatusRepository : IPlayerStatusRepository
    {
        private float _limitAngleDeg;

        public void SetLimitAngle(float angleDeg)
        {
            _limitAngleDeg = angleDeg;
        }

        public void AddLimitAngle(float deltaDeg)
        {
            _limitAngleDeg += deltaDeg;
        }

        public float GetLimitAngle() => _limitAngleDeg;
    }
}
