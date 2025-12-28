using Core.Common;
using Core.Common.Messaging;
using Core.Money;

namespace OneTripMover.UseCase
{
    public class MoneyEventHandler : IMoneyEventHandler
    {
        private IMoneyUseCase _moneyUseCase;

        public MoneyEventHandler()
        {
            _moneyUseCase = ServiceLocator.Resolve<IMoneyUseCase>();

            var transactionSub = ServiceLocator.Resolve<ISubscriber<MoneyTransactionEvent>>();
            transactionSub.Subscribe(OnMoneyTransaction);
        }

        private void OnMoneyTransaction(MoneyTransactionEvent evt)
        {
            // 今後UI用の追加処理を入れる場合はここに
        }
    }
}
