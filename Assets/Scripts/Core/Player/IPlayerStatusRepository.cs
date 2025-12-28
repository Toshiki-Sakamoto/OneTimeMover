namespace Core.Player
{
    public interface IPlayerStatusRepository
    {
        void SetLimitAngle(float angleDeg);
        void AddLimitAngle(float deltaDeg);
        float GetLimitAngle();
    }
}
