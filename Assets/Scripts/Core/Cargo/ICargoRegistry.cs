using OneTripMover.Core;

namespace Core.Cargo
{
    public interface ICargoRegistry
    {
        void Register(ICargo cargo);

        void Unregister(ICargo cargo);
        
        void Clear();

        /// <summary>
        /// 現在の最上位の荷物を取得
        /// </summary>
        ICargo GetTopCargo();

        bool TryGet(CargoId cargoId, out ICargo cargo);
    }
}
