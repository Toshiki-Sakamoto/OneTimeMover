using Core;
using Core.Common;
using Core.Common.Messaging;
using Core.Money;

namespace UI.Money
{
    public class MoneyUIEventHandler : IEventHandler
    {
        private readonly MoneyUIController _controller;

        public MoneyUIEventHandler()
        {
            _controller = ServiceLocator.Resolve<MoneyUIController>();

            var applySub = ServiceLocator.Resolve<ISubscriber<MoneyAnimationApplyEvent>>();
            applySub.Subscribe(OnApply);
            
            var moneyChangedSubscriber = ServiceLocator.Resolve<ISubscriber<MoneyChangedEvent>>();
            moneyChangedSubscriber.Subscribe(OnMoneyChanged);
        }

        private void OnApply(MoneyAnimationApplyEvent evt)
        {
            _controller.Apply(evt);
        }

        private void OnMoneyChanged(MoneyChangedEvent evt)
        {
            if (!evt.Immediately) return;
            
            _controller.SetAmount(evt.Current);
        }
    }
}
