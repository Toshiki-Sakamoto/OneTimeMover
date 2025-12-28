using Core.Player;
using OneTripMover.Master;
using UnityEngine;

namespace Master
{
    [CreateAssetMenu(fileName = "PlayerMaster", menuName = "OneTripMover/Master/PlayerMaster")]
    public class PlayerMaster : AddressableMasterData<IPlayerMaster>, IPlayerMaster
    {
        [SerializeField] private float _limitAngleDeg = 30f;
        [SerializeField] private int _initialMoney = 0;
        [SerializeField] private float _moveForce = 1000f;
        [SerializeField] private float _maxMoveSpeed = 0.3f;
        
        public float LimitAngleDeg => _limitAngleDeg;
        public int InitialMoney => _initialMoney;
        public float MoveForce => _moveForce;
        public float MaxMoveSpeed => _maxMoveSpeed;
    }
}
