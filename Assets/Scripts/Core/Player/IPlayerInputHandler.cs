using UnityEngine;

namespace OneTripMover.Core.Player
{
    public interface IPlayerInputHandler
    {
        public void OnPlayerMoveInput(Vector2 moveInput);
        public void OnPlayerBalanceInput(float balanceInput);
    }
}