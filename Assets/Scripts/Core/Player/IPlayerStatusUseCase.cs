namespace Core.Player
{
    public interface IPlayerStatusUseCase
    {
        void InitializeFromMaster();
        void SetLimitAngle(float angleDeg);
        void AddLimitAngle(float deltaDeg);
        float GetLimitAngle();
    }
}
