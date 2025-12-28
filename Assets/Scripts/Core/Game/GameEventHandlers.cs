using Core.Common;
using OneTripMover.UseCase;
using UI.Adventure;
using UI.Bonus;
using UI.Money;

namespace Core.Game
{
    public class GameEventHandlers : IGameEventHandlers
    {
        private IStageEventHandler _stageEventHandler;
        private IMoneyEventHandler _moneyEventHandler;
        private ICargoEventHandler _cargoEventHandler;
        private UI.GameOver.GameOverUIViewEventHandler _gameOverEventHandler;
        private AdventureUIEventHandler _adventureUIEventHandler;
        private OneTripMover.Views.Stage.SettleViewEventHandler _settleViewEventHandler;
        private BonusUIEventHandler _bonusUIEventHandler;
        private BonusStatusEventHandler _bonusStatusEventHandler;
        private MoneyAnimationUIEventHandler _moneyAnimationEventHandler;
        private MoneyUIEventHandler _moneyUIEventHandler;
        private OneTripMover.Views.Stage.SettlementRewardEventHandler _settlementRewardEventHandler;
        private BonusPresentationFinishedEvent _bonusPresentationFinishedEvent;
        
        public GameEventHandlers()
        {
            _stageEventHandler = ServiceLocator.Resolve<IStageEventHandler>();
            _moneyEventHandler = ServiceLocator.Resolve<IMoneyEventHandler>();
            _cargoEventHandler = ServiceLocator.Resolve<ICargoEventHandler>();
            _gameOverEventHandler = ServiceLocator.Resolve<UI.GameOver.GameOverUIViewEventHandler>();
            _adventureUIEventHandler = ServiceLocator.Resolve<AdventureUIEventHandler>();
            _settleViewEventHandler = ServiceLocator.Resolve<OneTripMover.Views.Stage.SettleViewEventHandler>();
            _bonusUIEventHandler = ServiceLocator.Resolve<BonusUIEventHandler>();
            _bonusStatusEventHandler = ServiceLocator.Resolve<BonusStatusEventHandler>();
            _moneyAnimationEventHandler = ServiceLocator.Resolve<MoneyAnimationUIEventHandler>();
            _moneyUIEventHandler = ServiceLocator.Resolve<MoneyUIEventHandler>();
            _settlementRewardEventHandler = ServiceLocator.Resolve<OneTripMover.Views.Stage.SettlementRewardEventHandler>();
            ServiceLocator.Resolve<BonusPresentationFinishedEvent>();
        }
    }
}
