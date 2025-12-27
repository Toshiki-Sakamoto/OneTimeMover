using UnityEngine;

namespace OneTripMover.Input
{
    public class PlayerInputEvent
    {
        public readonly Vector2 MoveDirection;
        public readonly float Balance;

        public PlayerInputEvent(Vector2 moveDirection, float balance)
        {
            MoveDirection = moveDirection;
            Balance = balance;
        }
    }
}