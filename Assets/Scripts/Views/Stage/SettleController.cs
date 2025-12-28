using Core.Common;
using Core.Common.Messaging;
using Core.Game;
using UnityEngine;

namespace OneTripMover.Views.Stage
{
    /// <summary>
    /// 精算処理の開始を通知するView側コントローラ。
    /// </summary>
    public class SettleController : MonoBehaviour
    {
        private IPublisher<SettlementStartedEvent> _settlementStartedPublisher;

        private void Awake()
        {
            _settlementStartedPublisher = ServiceLocator.Resolve<IPublisher<SettlementStartedEvent>>();
        }

        public void StartSettlement()
        {
            _settlementStartedPublisher?.Publish(new SettlementStartedEvent());
        }
    }
}
