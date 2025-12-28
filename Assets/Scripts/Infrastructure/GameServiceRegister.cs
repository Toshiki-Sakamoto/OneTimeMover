using Core.Adventure;
using Core.Cargo;
using Core.Common;
using Core.Game;
using Core.Money;
using Core.Phase;
using Core.Player;
using Core.Stage;
using Master;
using OneTripMover.Asset;
using OneTripMover.Core;
using OneTripMover.Core.Entity;
using OneTripMover.Master;
using OneTripMover.UseCase;
using OneTripMover.Views.Stage;
using UI.Adventure;
using UI.Bonus;
using UI.GameOver;
using UI.Money;
using Views.Cargo;

namespace OneTripMover.Infrastructure
{
    public class GameServiceRegister : ServiceRegister
    {
        protected override void RegisterService()
        {
            ServiceLocatorExtensions.RegisterAllEventBuses();
            
            RegisterSingleton<IAsyncInitializable, GameEntry>();
            RegisterSingleton<IGameEventHandlers, GameEventHandlers>();
            RegisterSingleton<IStageEventHandler, StageEventHandler>();
            
            RegisterSingleton<ICargoMasterRegistry, CargoMasterRegistry>();
            RegisterSingleton<IStageMasterRegistry, StageMasterRegistry>();
            RegisterSingleton<IDefineMasterRegistry, DefineMasterRegistry>();
            RegisterSingleton<IMasterDataLoader, AddressableMasterDataLoader>();
            RegisterSingleton<IAssetLoader, AssetLoader>();
            RegisterSingleton<IAssetAutoReleaseService, AssetAutoReleaseService>();
            
            RegisterSingleton<IPhaseService<GamePhase>, GamePhaseService>();
            RegisterSingleton<IGamePhaseEntryExecutor, GamePhaseEntryExecutor>();
            RegisterSingleton<IGamePhaseInitializeExecutor, GameGamePhaseInitializeExecutor>();
            RegisterSingleton<IGamePhasePlayExecutor, GameGamePhasePlayExecutor>();
            RegisterSingleton<IGamePhaseClearExecutor, GameGamePhaseClearExecutor>();
         
            RegisterSingleton<IGamePhaseEntryHandler, GamePhaseEntryHandler>();
            RegisterSingleton<IGamePhaseInitializeHandler, GamePhaseInitializeHandler>();
            RegisterSingleton<IGamePhasePlayHandler, GamePhasePlayHandler>();
            RegisterSingleton<IGamePhaseClearHandler, GamePhaseClearHandler>();
            RegisterSingleton<ICargoFactory, CargoFactory>();
            
            RegisterSingleton<IStageUseCase, StageUseCase>();
            RegisterSingleton<IStageRepository, StageRepository>();
            RegisterSingleton<ICargoRegistry, CargoRegistry>();
            RegisterSingleton<ICargoUseCase, CargoUseCase>();
            RegisterSingleton<IMoneyRepository, MoneyRepository>();
            RegisterSingleton<IMoneyUseCase, MoneyUseCase>();
            RegisterSingleton<IMoneyEventHandler, MoneyEventHandler>();
            RegisterSingleton<ICargoEventHandler, CargoEventHandler>();
            RegisterSingleton<BonusStatusEventHandler, BonusStatusEventHandler>();
            RegisterSingleton<IGameOverUIController, GameOverUIController>();
            RegisterSingleton<GameOverUIViewEventHandler, GameOverUIViewEventHandler>();
            RegisterSingleton<IAdventureService, AdventureService>();
            RegisterSingleton<AdventureUIController, AdventureUIController>();
            RegisterSingleton<AdventureUIEventHandler, AdventureUIEventHandler>();
            RegisterSingleton<BonusUIController, BonusUIController>();
            RegisterSingleton<BonusUIEventHandler, BonusUIEventHandler>();
            RegisterSingleton<MoneyAnimationUIController, MoneyAnimationUIController>();
            RegisterSingleton<MoneyAnimationUIEventHandler, MoneyAnimationUIEventHandler>();
            RegisterSingleton<MoneyUIController, MoneyUIController>();
            RegisterSingleton<MoneyUIEventHandler, MoneyUIEventHandler>();
            RegisterSingleton<UI.Bonus.BonusPresentationFinishedEvent, UI.Bonus.BonusPresentationFinishedEvent>();
            RegisterSingleton<InputSystem_Actions, InputSystem_Actions>();
            RegisterSingleton<OneTripMover.Input.IInputContextUseCase, OneTripMover.Input.InputContextUseCase>();
            RegisterSingleton<IPlayerStatusRepository, PlayerStatusRepository>();
            RegisterSingleton<IPlayerStatusUseCase, PlayerStatusUseCase>();
            RegisterSingleton<IPlayerMasterRegistry, PlayerMasterRegistry>();
            RegisterSingleton<SettleViewEventHandler, SettleViewEventHandler>();
            RegisterSingleton<SettlementController, SettlementController>();
            RegisterSingleton<SettlementRewardEventHandler, SettlementRewardEventHandler>();
            RegisterSingleton<ICargoViewRegistry, CargoViewRegistry>();

            RegisterSingleton<IIdGeneratorStateRepository, IdGeneratorStateRepository>();
            RegisterSingleton<IIdGeneratorRegistry, IdGeneratorRegistry>();
            RegisterSingleton<IIdGenerator<Cargo>, TypeSequentialIdGenerator<Cargo>>();
            RegisterSingleton<IEntityForIdFactory<Cargo, Cargo>, EntityForIdFactory<Cargo, Cargo>>();

            RegisterComponentInHierarchy<GameOverUIView>();
            RegisterComponentInHierarchy<IAdventureUIView>();
            RegisterComponentInHierarchy<SettleController>();
            RegisterComponentInHierarchy<BonusUIView>();
            RegisterComponentInHierarchy<MoneyAnimationUIView>();
            RegisterComponentInHierarchy<MoneyUIView>();
            RegisterComponentInHierarchy<SettlementController>();
            RegisterComponentInHierarchy<GoalController>();
        }
    }
}
