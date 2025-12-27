using Core.Common;
using Core.Common.Messaging;
using Core.Game;
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
        private IPublisher<PlayerInputEnableEvent> _playerInputEnablePublisher;
        
        [Inject]
        public void Construct()
        {
            _playerController = GetComponent<PlayerController>();
            
            _cargoMasterRegistry = ServiceLocator.Resolve<ICargoMasterRegistry>();
            _playerInputEnablePublisher = ServiceLocator.Resolve<IPublisher<PlayerInputEnableEvent>>();
            
            var cargoAddedSubscriber = ServiceLocator.Resolve<ISubscriber<CargoAddedEvent>>();
            var gameStartedSubscriber = ServiceLocator.Resolve<ISubscriber<GameStartedEvent>>();
            var gamePhaseWillEnterSubscriber = ServiceLocator.Resolve<ISubscriber<GamePhaseWillEnterEvent>>();
            var dangerLeanSubscriber = ServiceLocator.Resolve<ISubscriber<CargoTowerDangerLeanEvent>>();
            var dangerClearedSubscriber = ServiceLocator.Resolve<ISubscriber<CargoTowerDangerClearedEvent>>();
            
            cargoAddedSubscriber.Subscribe(OnCargoAdded);
            gameStartedSubscriber.Subscribe(OnGameStarted);
            gamePhaseWillEnterSubscriber.Subscribe(OnGamePhaseWillEnter);
            dangerLeanSubscriber.Subscribe(OnCargoTowerDangerLean);
            dangerClearedSubscriber.Subscribe(OnCargoTowerDangerCleared);
        }
        
        private void OnCargoAdded(CargoAddedEvent evt)
        {
            if (_cargoMasterRegistry.TryGetByCargoId(evt.Cargo.CargoId, out var cargoMaster))
            {
                var tower = _playerController.CargoStackTower;
                tower.AddCargoView(cargoMaster);
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

        private void OnGamePhaseWillEnter(GamePhaseWillEnterEvent evt)
        {
            switch (evt.NextPhase)
            {
                case GamePhase.Initialize:
                {
                    _playerInputEnablePublisher.Publish(new PlayerInputEnableEvent { Enabled = false });
                    break;
                }
            }
        }
    }
}
