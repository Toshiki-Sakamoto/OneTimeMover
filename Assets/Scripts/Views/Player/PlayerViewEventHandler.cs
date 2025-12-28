using Core.Common;
using Core.Common.Messaging;
using Core.Game;
using Core.Player;
using OneTripMover.Input;
using OneTripMover.Master;
using OneTripMover.UseCase;
using UnityEngine;

namespace OneTripMover.Views.Player
{
    public class PlayerViewEventHandler : MonoBehaviour
    {
        private ICargoMasterRegistry _cargoMasterRegistry;
        private PlayerController _playerController;
        private IPlayerMasterRegistry _playerMasterRegistry;
        private IPublisher<PlayerInputEnableEvent> _playerInputEnablePublisher;
        
        [Inject]
        public void Construct()
        {
            _playerController = GetComponent<PlayerController>();
            
            _cargoMasterRegistry = ServiceLocator.Resolve<ICargoMasterRegistry>();
            _playerMasterRegistry = ServiceLocator.Resolve<IPlayerMasterRegistry>();
            _playerInputEnablePublisher = ServiceLocator.Resolve<IPublisher<PlayerInputEnableEvent>>();
            
            var cargoAddedSubscriber = ServiceLocator.Resolve<ISubscriber<CargoAddedEvent>>();
            var gameStartedSubscriber = ServiceLocator.Resolve<ISubscriber<GameStartedEvent>>();
            var gamePhaseWillEnterSubscriber = ServiceLocator.Resolve<ISubscriber<GamePhaseWillEnterEvent>>();
            var dangerLeanSubscriber = ServiceLocator.Resolve<ISubscriber<CargoTowerDangerLeanEvent>>();
            var dangerClearedSubscriber = ServiceLocator.Resolve<ISubscriber<CargoTowerDangerClearedEvent>>();
            var goalClearedSubscriber = ServiceLocator.Resolve<ISubscriber<GoalClearedEvent>>();
            
            cargoAddedSubscriber.Subscribe(OnCargoAdded);
            gameStartedSubscriber.Subscribe(OnGameStarted);
            gamePhaseWillEnterSubscriber.Subscribe(OnGamePhaseWillEnter);
            dangerLeanSubscriber.Subscribe(OnCargoTowerDangerLean);
            dangerClearedSubscriber.Subscribe(OnCargoTowerDangerCleared);
            goalClearedSubscriber.Subscribe(OnGoalCleared);
        }
        
        private void OnCargoAdded(CargoAddedEvent evt)
        {
            if (_cargoMasterRegistry.TryGetValue(evt.Cargo.MasterId, out var cargoMaster))
            {
                var tower = _playerController.CargoStackTower;
                tower.AddCargoView(evt.Cargo.Id, cargoMaster);
            }
        }
        
        private void OnGameStarted(GameStartedEvent evt)
        {
            // 現在の積みあがっている荷物に開始命令を送る
            var tower = _playerController.CargoStackTower;
        }

        private void OnCargoTowerDangerLean(CargoTowerDangerLeanEvent evt)
        {
            if (evt.Player != _playerController) return;
            // TODO: 危険表示や警告音などのリアクションをここに追加
        }

        private void OnCargoTowerDangerCleared(CargoTowerDangerClearedEvent evt)
        {
            if (evt.Player != _playerController) return;
            // TODO: 危険表示の解除などをここに追加
        }

        private void OnGoalCleared(GoalClearedEvent evt)
        {
            if (evt.Player != _playerController) return;
            
            _playerController.CargoStackTower?.OnGoalCleared(evt);
        }

        private void OnGamePhaseWillEnter(GamePhaseWillEnterEvent evt)
        {
            switch (evt.NextPhase)
            {
                case GamePhase.Entry:
                {
                    _playerController.PlayerMover.ApplyMasterSettings(_playerMasterRegistry.GetMaster());
                    break;
                }
                
                case GamePhase.Initialize:
                {
                    _playerInputEnablePublisher.Publish(new PlayerInputEnableEvent { Enabled = false });
                    break;
                }
            }
        }
    }
}
