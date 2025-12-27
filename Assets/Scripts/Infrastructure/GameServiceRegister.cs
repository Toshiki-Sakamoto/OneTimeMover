using Core.Cargo;
using Core.Common;
using Core.Game;
using Core.Phase;
using Core.Stage;
using Master;
using OneTripMover.Asset;
using OneTripMover.Core;
using OneTripMover.Master;
using OneTripMover.UseCase;

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
        }
    }
}
