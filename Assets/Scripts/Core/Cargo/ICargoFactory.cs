namespace Core.Cargo
{
    public interface ICargoFactory
    {
        ICargo Create(ICargoMaster master, bool isOneMoreBonus = false);
    }
}
