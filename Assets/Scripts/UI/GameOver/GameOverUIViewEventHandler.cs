using Core.Common;
using Core.Common.Messaging;
using Core.Money;

namespace UI.GameOver
{
    public class GameOverUIViewEventHandler : Core.IEventHandler
    {
        private IGameOverUIController _controller;

        public GameOverUIViewEventHandler()
        {
            _controller = ServiceLocator.Resolve<IGameOverUIController>();

            var moneyDepleted = ServiceLocator.Resolve<ISubscriber<MoneyDepletedEvent>>();
            moneyDepleted.Subscribe(OnMoneyDepleted);
        }

        private void OnMoneyDepleted(MoneyDepletedEvent evt)
        {
            _controller?.Play();
        }
    }
}
