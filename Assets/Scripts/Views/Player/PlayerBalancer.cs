using UnityEngine;

namespace OneTripMover.Views.Player
{
    public interface IPlayerBalanceHandler
    {
        void AddBalanceForce(Vector2 force);
    }
    
    public class PlayerBalancer : MonoBehaviour, IPlayerBalanceInputHandler
    {
        [SerializeField] private float balanceNudge = 2.5f;
        private IPlayerBalanceHandler _handler;

        public void SetBalanceHandler(IPlayerBalanceHandler handler)
        {
            _handler = handler;
        }
        
        public void SetBalanceInput(float input)
        {
            if (Mathf.Approximately(input, 0f)) return;
            
            // タワーの上の方に横方向の軽い力を加え、揺れへ対抗
            _handler?.AddBalanceForce(Vector2.right * (input * balanceNudge));
        }
    }
}