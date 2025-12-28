namespace Core.Player
{
    public interface IPlayerStatusUseCase
    {
        void SetLimitAngle(float angleDeg);
        void AddLimitAngle(float deltaDeg);
        float GetLimitAngle();
    }
}
