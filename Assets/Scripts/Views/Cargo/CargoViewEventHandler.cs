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
        
        [Inject]
        public void Construct()
        {
            _gamePhaseService = ServiceLocator.Resolve<IPhaseService<GamePhase>>();
            
            var cargoViewBreakSubscriber = ServiceLocator.Resolve<ISubscriber<CargoJointViewBreakEvent>>();
            cargoViewBreakSubscriber.Subscribe(OnCargoJointViewBreak);
        }
        
        private void OnCargoJointViewBreak(CargoJointViewBreakEvent evt)
        {
            if (_gamePhaseService.Current != GamePhase.Play) return;

            evt.CargoView.Break();
        }
    }
}