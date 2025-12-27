using Core.Phase;

namespace Core.Game
{
    // GamePhase用のエイリアス（既存互換）
    public interface IGamePhaseExecutor : IPhaseExecutor<GamePhase> {}
    public interface IGamePhaseEntryExecutor : IGamePhaseExecutor {}
    public interface IGamePhaseInitializeExecutor : IGamePhaseExecutor {}
    public interface IGamePhaseStartExecutor : IGamePhaseExecutor {}
    public interface IGamePhasePlayExecutor : IGamePhaseExecutor {}
    public interface IGamePhasePauseExecutor : IGamePhaseExecutor {}
    public interface IGamePhaseOverExecutor : IGamePhaseExecutor {}
    public interface IGamePhaseClearExecutor : IGamePhaseExecutor {}
    
    // GamePhase用のエイリアスインターフェイス群（既存互換）
    public interface IGamePhaseHandler : IPhaseHandler<GamePhase> {}
    public interface IGamePhaseEntryHandler : IGamePhaseHandler {}
    public interface IGamePhaseInitializeHandler : IGamePhaseHandler {}
    public interface IGamePhaseStartHandler : IGamePhaseHandler {}
    public interface IGamePhasePlayHandler : IGamePhaseHandler {}
    public interface IGamePhasePauseHandler : IGamePhaseHandler {}
    public interface IGamePhaseOverHandler : IGamePhaseHandler {}
    public interface IGamePhaseClearHandler : IGamePhaseHandler {}
}