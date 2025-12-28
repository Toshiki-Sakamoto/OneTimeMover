using Core;
using Core.Common;
using Core.Common.Messaging;
using Core.Game;

namespace OneTripMover.Views.Stage
{
    public class SettleViewEventHandler : IEventHandler
    {
        private readonly SettleController _settleController;

        public SettleViewEventHandler()
        {
            _settleController = ServiceLocator.Resolve<SettleController>();

            var clearAnimFinishedSub = ServiceLocator.Resolve<ISubscriber<GoalClearAnimationFinishedEvent>>();
            clearAnimFinishedSub.SubscribeDisposable(OnGoalClearAnimationFinished);

            var settlementStartedSub = ServiceLocator.Resolve<ISubscriber<SettlementStartedEvent>>();
            settlementStartedSub.Subscribe(OnSettlementStarted);
        }

        private void OnGoalClearAnimationFinished(GoalClearAnimationFinishedEvent evt)
        {
            _settleController?.StartSettlement();
        }

        private void OnSettlementStarted(SettlementStartedEvent evt)
        {
            // TODO: Settle演出の開始処理をここに追加
        }
    }
}
