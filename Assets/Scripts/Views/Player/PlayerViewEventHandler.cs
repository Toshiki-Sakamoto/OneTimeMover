using Core.Common;
using Core.Common.Messaging;
using Core.Game;
using OneTripMover.Master;
using OneTripMover.UseCase;
using UnityEngine;

namespace OneTripMover.Views.Player
{
    public class PlayerViewEventHandler : MonoBehaviour
    {
        private ICargoMasterRegistry _cargoMasterRegistry;
        private PlayerController _playerController;
        
        [Inject]
        public void Construct()
        {
            _playerController = GetComponent<PlayerController>();
            
            _cargoMasterRegistry = ServiceLocator.Resolve<ICargoMasterRegistry>();
            var cargoAddedSubscriber = ServiceLocator.Resolve<ISubscriber<CargoAddedEvent>>();
            var gameStartedSubscriber = ServiceLocator.Resolve<ISubscriber<GameStartedEvent>>();
            
            cargoAddedSubscriber.Subscribe(OnCargoAdded);
            gameStartedSubscriber.Subscribe(OnGameStarted);
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
    }
}