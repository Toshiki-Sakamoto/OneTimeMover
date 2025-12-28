using Core;
using Core.Common;
using Core.Common.Messaging;
using Core.Money;

namespace UI.Money
{
    public class MoneyAnimationUIEventHandler : IEventHandler
    {
        private readonly MoneyAnimationUIController _controller;

        public MoneyAnimationUIEventHandler()
        {
            _controller = ServiceLocator.Resolve<MoneyAnimationUIController>();

            var moneyChangedSubscriber = ServiceLocator.Resolve<ISubscriber<MoneyChangedEvent>>();
            moneyChangedSubscriber.Subscribe(OnMoneyChanged);
        }

        private void OnMoneyChanged(MoneyChangedEvent evt)
        {
            if (evt.Immediately) return;
            
            var delta = evt.Current - evt.Previous;
            if (delta == 0) return;
            _controller.Play(delta, evt.Current);
        }
    }
}
