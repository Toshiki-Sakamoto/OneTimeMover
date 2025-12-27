using Core.Common;
using Core.Common.Messaging;
using Core.Phase;
using OneTripMover.Input;

namespace Core.Game
{
    public class GamePhasePlayHandler : IGamePhasePlayHandler
    {
        private readonly IPublisher<GameStartedEvent> _gameStartedEventPublisher;
        private readonly IPublisher<PlayerInputEnableEvent> _playerInputEnableEventPublisher;

        public GamePhasePlayHandler()
        {
            _gameStartedEventPublisher = ServiceLocator.Resolve<IPublisher<GameStartedEvent>>();
            _playerInputEnableEventPublisher = ServiceLocator.Resolve<IPublisher<PlayerInputEnableEvent>>();
        }
        
        public void OnEnter(IPhaseContext<GamePhase> context)
        {
            _playerInputEnableEventPublisher.Publish(new PlayerInputEnableEvent { Enabled = true });
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
