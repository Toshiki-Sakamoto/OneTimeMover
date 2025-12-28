
using System.Threading.Tasks;
using Core.Common;
using Core.Common.Messaging;
using Core.Game;
using Core.Phase;
using Core.Player;
using Core.Money;
using OneTripMover.Master;
using OneTripMover.Views.Player;
using Unity.VisualScripting;
using IInitializable = Core.Common.IInitializable;

namespace OneTripMover.Core
{
    public class GameEntry : IInitializable, IAsyncInitializable, IUpdatable
    {
        private IMasterDataLoader _masterDataLoader;
        private IPhaseService<GamePhase> _gamePhaseService;
        private IGameEventHandlers _gameEventHandlers;
        private IMoneyUseCase _moneyUseCase;

        public void Initialize()
        {
            _masterDataLoader = ServiceLocator.Resolve<IMasterDataLoader>();
            _gamePhaseService = ServiceLocator.Resolve<IPhaseService<GamePhase>>();
            _gameEventHandlers = ServiceLocator.Resolve<IGameEventHandlers>();
            _moneyUseCase = ServiceLocator.Resolve<IMoneyUseCase>();
            
            _gamePhaseService.Initialize();
        }

        public async Task InitializeAsync()
        {
            await _masterDataLoader.LoadAsync();
            
            _gamePhaseService.ChangePhase(GamePhase.Entry);
        }

        public void Update(float deltaTime)
        {
            _gamePhaseService.Update(deltaTime);
        }
    }
}
