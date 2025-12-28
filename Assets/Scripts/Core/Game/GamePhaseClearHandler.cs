using Core.Common;
using Core.Common.Messaging;
using Core.Phase;
using OneTripMover.Input;

namespace Core.Game
{
    public class GamePhaseClearHandler : IGamePhaseClearHandler
    {
        private readonly IPublisher<PlayerInputEnableEvent> _playerInputPublisher;
        private readonly IInputContextUseCase _inputContextUseCase;

        public GamePhaseClearHandler()
        {
            _playerInputPublisher = ServiceLocator.Resolve<IPublisher<PlayerInputEnableEvent>>();
            _inputContextUseCase = ServiceLocator.Resolve<IInputContextUseCase>();
        }

        public void OnEnter(IPhaseContext<GamePhase> context)
        {
            _inputContextUseCase?.SetUIContext();
            _playerInputPublisher?.Publish(new PlayerInputEnableEvent { Enabled = false });
        }

        public void OnUpdate(IPhaseContext<GamePhase> context, float deltaTime)
        {
        }

        public void OnExit(IPhaseContext<GamePhase> context)
        {
        }
    }
}
