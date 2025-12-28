using Core.Player;
using OneTripMover.Master;
using UnityEngine;

namespace Master
{
    [CreateAssetMenu(fileName = "PlayerMaster", menuName = "OneTripMover/Master/PlayerMaster")]
    public class PlayerMaster : AddressableMasterData<IPlayerMaster>, IPlayerMaster
    {
        [SerializeField] private float _limitAngleDeg = 30f;
        public float LimitAngleDeg => _limitAngleDeg;
    }
}
