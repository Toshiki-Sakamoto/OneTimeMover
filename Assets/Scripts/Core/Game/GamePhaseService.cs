using Core.Common;
using Core.Common.Messaging;
using Core.Phase;

namespace Core.Game
{
    /// <summary>
    /// ゲーム中のフェーズ管理サービス
    /// </summary>
    public class GamePhaseService : PhaseService<GamePhase>
    {
        private IPublisher<GamePhaseWillEnterEvent> _willEnterPublisher;

        protected override void OnBeforeEnter(GamePhase nextPhase)
        {
            _willEnterPublisher ??= ServiceLocator.Resolve<IPublisher<GamePhaseWillEnterEvent>>();
            _willEnterPublisher?.Publish(new GamePhaseWillEnterEvent
            {
                NextPhase = nextPhase,
                PreviousPhase = Current
            });
        }
    }
}
