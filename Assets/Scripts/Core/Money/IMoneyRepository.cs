namespace Core.Money
{
    public interface IMoneyRepository
    {
        void SetMoney(int amount);
        void AddMoney(int amount);
        void SubtractMoney(int amount);
        int GetMoney();
    }
}
