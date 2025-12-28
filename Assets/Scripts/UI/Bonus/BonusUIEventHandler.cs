using Core;
using Core.Common;
using Core.Common.Messaging;
using Core.Stage;
using Core.Game;

namespace UI.Bonus
{
    public class BonusUIEventHandler : IEventHandler
    {
        private readonly BonusUIController _controller;

        public BonusUIEventHandler()
        {
            _controller = ServiceLocator.Resolve<BonusUIController>();

            var perfectSub = ServiceLocator.Resolve<ISubscriber<PerfectBonusChangedEvent>>();
            perfectSub.Subscribe(OnPerfectChanged);

            var oneMoreSub = ServiceLocator.Resolve<ISubscriber<OneMoreBonusChangedEvent>>();
            oneMoreSub.Subscribe(OnOneMoreChanged);

            var settlementFinishedSub = ServiceLocator.Resolve<ISubscriber<SettlementFinishedEvent>>();
            settlementFinishedSub.Subscribe(OnSettlementFinished);
        }

        private void OnPerfectChanged(PerfectBonusChangedEvent evt)
        {
            _controller.SetPerfect(evt.Enabled);
        }

        private void OnOneMoreChanged(OneMoreBonusChangedEvent evt)
        {
            _controller.SetOneMore(evt.Enabled);
        }

        private void OnSettlementFinished(SettlementFinishedEvent evt)
        {
            // ボーナス演出を再生し、終了したらイベントを発行する
            _controller.PlayBonusPresentation();
        }
    }
}
