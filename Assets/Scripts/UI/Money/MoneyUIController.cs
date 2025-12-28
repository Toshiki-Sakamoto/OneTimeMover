using Core.Common;
using Core.Common.Messaging;
using Core.Money;

namespace UI.Money
{
    /// <summary>
    /// 所持金表示の更新を担当。
    /// </summary>
    public class MoneyUIController
    {
        private readonly MoneyUIView _view;
        private readonly IPublisher<MoneyChangedEvent> _changedPublisher;

        public MoneyUIController()
        {
            _view = ServiceLocator.Resolve<MoneyUIView>();
            _changedPublisher = ServiceLocator.Resolve<IPublisher<MoneyChangedEvent>>();
        }

        public void SetAmount(int current)
        {
            _view.SetAmount(current);
        }

        public void Apply(MoneyAnimationApplyEvent evt)
        {
            SetAmount(evt.Current);
        }
    }
}
