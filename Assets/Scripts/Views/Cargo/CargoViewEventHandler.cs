using Core.Common;
using Core.Common.Messaging;
using Core.Game;
using Core.Phase;
using OneTripMover.Views.Player;
using UnityEngine;

namespace Views.Cargo
{
    public class CargoViewEventHandler : MonoBehaviour
    {
        private IPhaseService<GamePhase> _gamePhaseService;
        private OneTripMover.Views.Player.PlayerCargoTowerController _tower;
        
        [Inject]
        public void Construct()
        {
            _gamePhaseService = ServiceLocator.Resolve<IPhaseService<GamePhase>>();
            _tower = FindFirstObjectByType<OneTripMover.Views.Player.PlayerCargoTowerController>();
            
            var cargoViewBreakSubscriber = ServiceLocator.Resolve<ISubscriber<CargoJointViewBreakEvent>>();
            cargoViewBreakSubscriber.Subscribe(OnCargoJointViewBreak);

            var phaseSub = ServiceLocator.Resolve<ISubscriber<GamePhaseWillEnterEvent>>();
            phaseSub.Subscribe(OnGamePhaseWillEnter);
        }
        
        private void OnCargoJointViewBreak(CargoJointViewBreakEvent evt)
        {
            if (_gamePhaseService.Current != GamePhase.Play) return;

            evt.CargoView.Break();
        }

        private void OnGamePhaseWillEnter(GamePhaseWillEnterEvent evt)
        {
            if (_tower == null) return;
            if (evt.NextPhase == GamePhase.Clear)
            {
                _tower.SetDangerIndicatorActive(false);
            }
            else if (evt.NextPhase == GamePhase.Initialize)
            {
                _tower.SetDangerIndicatorActive(true);
            }
        }
    }
}
