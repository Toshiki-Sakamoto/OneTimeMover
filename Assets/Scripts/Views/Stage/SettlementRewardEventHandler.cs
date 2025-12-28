using Core;
using Core.Common;
using Core.Common.Messaging;
using Core.Game;
using Core.Money;

namespace OneTripMover.Views.Stage
{
    public class SettlementRewardEventHandler : IEventHandler
    {
        private readonly IMoneyUseCase _moneyUseCase;
        private readonly IPublisher<MoneyTransactionEvent> _moneyTransactionPublisher;

        public SettlementRewardEventHandler()
        {
            _moneyUseCase = ServiceLocator.Resolve<IMoneyUseCase>();
            _moneyTransactionPublisher = ServiceLocator.Resolve<IPublisher<MoneyTransactionEvent>>();

            var rewardSub = ServiceLocator.Resolve<ISubscriber<SettlementRewardEvent>>();
            rewardSub.Subscribe(OnSettlementReward);
        }

        private void OnSettlementReward(SettlementRewardEvent evt)
        {
            if (evt == null) return;
            if (evt.Amount <= 0) return;

            _moneyUseCase.AddMoney(evt.Amount);
            _moneyTransactionPublisher?.Publish(new MoneyTransactionEvent
            {
                Delta = evt.Amount,
                Current = _moneyUseCase.GetMoney(),
                IsPenalty = false
            });
        }
    }
}
