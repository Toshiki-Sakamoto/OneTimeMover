using Core.Cargo;

namespace OneTripMover.UseCase
{
    /// <summary>
    /// 荷物追加イベント
    /// </summary>
    public class CargoAddedEvent
    {
        public ICargo Cargo;
    }
}