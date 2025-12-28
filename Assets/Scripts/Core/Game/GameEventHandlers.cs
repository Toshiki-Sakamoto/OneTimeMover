using Core.Common;
using OneTripMover.UseCase;
using UI.Adventure;

namespace Core.Game
{
    public class GameEventHandlers : IGameEventHandlers
    {
        private IStageEventHandler _stageEventHandler;
        private IMoneyEventHandler _moneyEventHandler;
        private ICargoEventHandler _cargoEventHandler;
        private UI.GameOver.GameOverUIViewEventHandler _gameOverEventHandler;
        private AdventureUIEventHandler _adventureUIEventHandler;
        
        public GameEventHandlers()
        {
            _stageEventHandler = ServiceLocator.Resolve<IStageEventHandler>();
            _moneyEventHandler = ServiceLocator.Resolve<IMoneyEventHandler>();
            _cargoEventHandler = ServiceLocator.Resolve<ICargoEventHandler>();
            _gameOverEventHandler = ServiceLocator.Resolve<UI.GameOver.GameOverUIViewEventHandler>();
            _adventureUIEventHandler = ServiceLocator.Resolve<AdventureUIEventHandler>();
        }
    }
}
