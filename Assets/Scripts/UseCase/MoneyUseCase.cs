using Core.Common;
using Core.Common.Messaging;
using Core.Money;
using System;

namespace OneTripMover.UseCase
{
    public class MoneyUseCase : IMoneyUseCase
    {
        private readonly IMoneyRepository _repository;
        private readonly IPublisher<MoneyChangedEvent> _changedPublisher;
        private readonly IPublisher<MoneyDepletedEvent> _depletedPublisher;

        public MoneyUseCase()
        {
            _repository = ServiceLocator.Resolve<IMoneyRepository>();
            _changedPublisher = ServiceLocator.Resolve<IPublisher<MoneyChangedEvent>>();
            _depletedPublisher = ServiceLocator.Resolve<IPublisher<MoneyDepletedEvent>>();
        }

        public void SetMoney(int amount) => ApplyChange(amount);

        public void AddMoney(int amount) => ApplyChange(_repository.GetMoney() + amount);

        public void SubtractMoney(int amount) => ApplyChange(_repository.GetMoney() - amount);

        public int GetMoney() => _repository.GetMoney();

        private void ApplyChange(int next)
        {
            var prev = _repository.GetMoney();

            _repository.SetMoney(next);
            _changedPublisher.Publish(new MoneyChangedEvent
            {
                Previous = prev,
                Current = next
            });

            if (next <= 0 && prev > 0)
            {
                _depletedPublisher.Publish(new MoneyDepletedEvent
                {
                    Previous = prev,
                    Current = next
                });
            }
        }
    }
}
