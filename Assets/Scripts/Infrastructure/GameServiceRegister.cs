using Core.Cargo;
using Core.Common;
using Core.Adventure;
using Core.Game;
using Core.Phase;
using Core.Stage;
using Core.Money;
using Master;
using OneTripMover.Asset;
using OneTripMover.Core;
using OneTripMover.Master;
using OneTripMover.UseCase;
using UI.GameOver;
using UI.Adventure;

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
         //   RegisterSingleton<IPhaseStartExecutor, GamePhaseStartExecutor>();
            RegisterSingleton<IGamePhasePlayExecutor, GameGamePhasePlayExecutor>();
         //   RegisterSingleton<IPhasePauseExecutor, GamePhasePauseExecutor>();
         //   RegisterSingleton<IPhaseOverExecutor, GamePhaseOverExecutor>();
         //   RegisterSingleton<IPhaseClearExecutor, GamePhaseClearExecutor>();
         
            RegisterSingleton<IGamePhaseEntryHandler, GamePhaseEntryHandler>();
            RegisterSingleton<IGamePhaseInitializeHandler, GamePhaseInitializeHandler>();
            RegisterSingleton<IGamePhasePlayHandler, GamePhasePlayHandler>();
            RegisterSingleton<ICargoFactory, CargoFactory>();
            
            RegisterSingleton<IStageUseCase, StageUseCase>();
            RegisterSingleton<IStageRepository, StageRepository>();
            RegisterSingleton<ICargoRegistry, CargoRegistry>();
            RegisterSingleton<ICargoUseCase, CargoUseCase>();
            RegisterSingleton<IMoneyRepository, MoneyRepository>();
            RegisterSingleton<IMoneyUseCase, MoneyUseCase>();
            RegisterSingleton<IMoneyEventHandler, MoneyEventHandler>();
            RegisterSingleton<ICargoEventHandler, CargoEventHandler>();
            RegisterSingleton<IGameOverUIController, GameOverUIController>();
            RegisterSingleton<GameOverUIViewEventHandler, GameOverUIViewEventHandler>();
            RegisterSingleton<IAdventureService, AdventureService>();
            RegisterSingleton<AdventureUIController, AdventureUIController>();
            RegisterSingleton<InputSystem_Actions, InputSystem_Actions>();
            RegisterSingleton<OneTripMover.Input.IInputContextUseCase, OneTripMover.Input.InputContextUseCase>();

            // ヒエラルキー上のUIコンポーネントを検索登録
            RegisterComponentInHierarchy<GameOverUIView>();
            RegisterComponentInHierarchy<IAdventureUIView>();
        }
    }
}
