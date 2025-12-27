using Core.Common;
using Core.Common.Messaging;
using Core.Phase;

namespace Core.Game
{
    public class GamePhasePlayHandler : IGamePhasePlayHandler
    {
        private readonly IPublisher<GameStartedEvent> _gameStartedEventPublisher;

        public GamePhasePlayHandler()
        {
            _gameStartedEventPublisher = ServiceLocator.Resolve<IPublisher<GameStartedEvent>>();
        }
        
        public void OnEnter(IPhaseContext<GamePhase> context)
        {
            _gameStartedEventPublisher.Publish(new GameStartedEvent());
        }

        public void OnUpdate(IPhaseContext<GamePhase> context, float deltaTime)
        {
        }

        public void OnExit(IPhaseContext<GamePhase> context)
        {
        }
    }
}
