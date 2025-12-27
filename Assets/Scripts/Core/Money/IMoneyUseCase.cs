namespace Core.Money
{
    public interface IMoneyUseCase
    {
        void SetMoney(int amount);
        void AddMoney(int amount);
        void SubtractMoney(int amount);
        int GetMoney();
    }
}
