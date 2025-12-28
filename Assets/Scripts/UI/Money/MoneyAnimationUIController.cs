using Core.Common;
using Core.Common.Messaging;
using Core.Money;

namespace UI.Money
{
    public class MoneyAnimationUIController
    {
        private readonly MoneyAnimationUIView _view;
        private readonly IPublisher<MoneyAnimationApplyEvent> _applyPublisher;

        public MoneyAnimationUIController()
        {
            _view = ServiceLocator.Resolve<MoneyAnimationUIView>();
            _applyPublisher = ServiceLocator.Resolve<IPublisher<MoneyAnimationApplyEvent>>();
        }

        public void Play(int delta, int current)
        {
            if (_view == null) return;
            var item = _view.SpawnItem(delta);
            if (item == null) return;

            item.ApplyRequested += () => _applyPublisher?.Publish(new MoneyAnimationApplyEvent
            {
                Current = current,
                Delta = delta
            });

            item.Play(delta);
        }
    }
}
