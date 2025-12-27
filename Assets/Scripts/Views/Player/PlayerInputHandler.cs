using Core.Common;
using Core.Common.Messaging;
using OneTripMover.Input;
using UnityEngine;

namespace OneTripMover.Views.Player
{
    public interface IPlayerMovementInputHandler
    {
        void SetMovementInput(Vector2 input);
    }
    
    public interface IPlayerBalanceInputHandler
    {
        void SetBalanceInput(float input);
    }
    
    public class PlayerInputHandler : MonoBehaviour
    {
        private IPlayerMovementInputHandler _movementInputHandler;
        private IPlayerBalanceInputHandler _balanceInputHandler;
        
        [Inject]
        public void Construct(
            ISubscriber<PlayerInputEvent> playerInputHandler)
        {
            playerInputHandler.Subscribe(OnPlayerInput);
        }
        
        public void SetMovement(IPlayerMovementInputHandler movementInputHandler)
        {
            _movementInputHandler = movementInputHandler;
        }
        
        public void SetBalance(IPlayerBalanceInputHandler balanceInputHandler)
        {
            _balanceInputHandler = balanceInputHandler;
        }

        private void OnPlayerInput(PlayerInputEvent inputEvent)
        {
            _movementInputHandler?.SetMovementInput(inputEvent.MoveDirection);
            _balanceInputHandler?.SetBalanceInput(inputEvent.Balance);
        }
    }
}