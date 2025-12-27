using Core.Money;

namespace OneTripMover.Infrastructure
{
    public class MoneyRepository : IMoneyRepository
    {
        private int _current;

        public void SetMoney(int amount)
        {
            _current = amount;
        }

        public void AddMoney(int amount)
        {
            _current += amount;
        }

        public void SubtractMoney(int amount)
        {
            _current -= amount;
        }

        public int GetMoney() => _current;
    }
}
