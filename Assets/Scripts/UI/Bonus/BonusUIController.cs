using Core.Common;
using Core.Stage;
using System.Threading.Tasks;
using Core.Common.Messaging;
using Core.Money;

namespace UI.Bonus
{
    public class BonusUIController
    {
        private readonly BonusUIView _view;
        private readonly IStageUseCase _stageUseCase;
        private readonly IPublisher<BonusPresentationFinishedEvent> _finishedPublisher;
        private readonly IMoneyUseCase _moneyUseCase;
        private readonly IPublisher<MoneyTransactionEvent> _moneyTransactionPublisher;

        public BonusUIController()
        {
            _view = ServiceLocator.Resolve<BonusUIView>();
            _stageUseCase = ServiceLocator.Resolve<IStageUseCase>();
            _finishedPublisher = ServiceLocator.Resolve<IPublisher<BonusPresentationFinishedEvent>>();
            _moneyUseCase = ServiceLocator.Resolve<IMoneyUseCase>();
            _moneyTransactionPublisher = ServiceLocator.Resolve<IPublisher<MoneyTransactionEvent>>();
        }

        public void SetPerfect(bool active)
        {
            _view?.SetPerfectActive(active);
        }

        public void SetOneMore(bool active)
        {
            _view?.SetOneMoreActive(active);
        }

        public async Task PlayBonusPresentation()
        {
            var perfect = _stageUseCase.HasPerfectBonus();
            var oneMore = _stageUseCase.HasOneMoreBonus();

            if (perfect)
            {
                var amount = _stageUseCase.GetPerfectBonusAmount();
                var item = _view.PerfectBonusText;
                if (item != null)
                {
                    await item.PlayAndWait(() => GrantBonus(amount));
                }
            }

            if (oneMore)
            {
                var amount = _stageUseCase.GetOneMoreBonusAmount();
                var item = _view.OneMoreBonusText;
                if (item != null)
                {
                    await item.PlayAndWait(() => GrantBonus(amount));
                }
            }

            _finishedPublisher?.Publish(new BonusPresentationFinishedEvent());
        }

        private void GrantBonus(int amount)
        {
            if (amount == 0) return;
            _moneyUseCase.AddMoney(amount);
            _moneyTransactionPublisher?.Publish(new MoneyTransactionEvent
            {
                Delta = amount,
                Current = _moneyUseCase.GetMoney(),
                IsPenalty = false
            });
        }
    }
}
