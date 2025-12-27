using System.Threading;
using Core.Common;
using Core.Common.Messaging;
using OneTripMover.Input;
using UnityEngine;

namespace OneTripMover.Views.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMover _playerMover;
        [SerializeField] private PlayerMotion _playerMotion;
        [SerializeField] private PlayerInputHandler _inputHandler;
        [SerializeField] private PlayerBalancer _playerBalancer;
        [SerializeField] private PlayerCargoTowerController _cargoStackTower;

        public PlayerCargoTowerController CargoStackTower => _cargoStackTower;
        public CancellationToken CancellationToken { get; private set; }

        private void Awake()
        {
            _inputHandler.SetBalance(_playerBalancer);
            _inputHandler.SetMovement(_playerMover);
            
            CancellationToken = new CancellationToken();

            _cargoStackTower.SetCancellationToken(CancellationToken);
            _playerBalancer.SetBalanceHandler(_cargoStackTower);
        }
    }
}