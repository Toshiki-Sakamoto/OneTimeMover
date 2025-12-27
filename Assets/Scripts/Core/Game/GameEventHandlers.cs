using Core.Common;
using OneTripMover.UseCase;

namespace Core.Game
{
    public class GameEventHandlers : IGameEventHandlers
    {
        private IStageEventHandler _stageEventHandler;
        private IMoneyEventHandler _moneyEventHandler;
        private OneTripMover.UseCase.ICargoEventHandler _cargoEventHandler;
        private UI.GameOver.GameOverUIViewEventHandler _gameOverEventHandler;
        
        public GameEventHandlers()
        {
            _stageEventHandler = ServiceLocator.Resolve<IStageEventHandler>();
            _moneyEventHandler = ServiceLocator.Resolve<IMoneyEventHandler>();
            _cargoEventHandler = ServiceLocator.Resolve<OneTripMover.UseCase.ICargoEventHandler>();
            _gameOverEventHandler = ServiceLocator.Resolve<UI.GameOver.GameOverUIViewEventHandler>();
        }
    }
}
