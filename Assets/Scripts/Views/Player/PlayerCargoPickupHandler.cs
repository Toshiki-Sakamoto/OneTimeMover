using System.Collections.Generic;
using Core.Common;
using Core.Common.Messaging;
using UnityEngine;
using Views.Cargo;
using OneTripMover.Input;

namespace OneTripMover.Views.Player
{
    /// <summary>
    /// ドロップ済みの荷物に触れている状態で拾い入力を受けたら塔のトップに接続する。
    /// </summary>
    public class PlayerCargoPickupHandler : MonoBehaviour
    {
        [SerializeField] private PlayerCargoTowerController _cargoTower;

        private readonly HashSet<CargoDropItemController> _touchingItems = new();
        private PlayerController _playerController;

        [Inject]
        public void Construct(
            ISubscriber<CargoDropItemTouchedEvent> touchedSubscriber,
            ISubscriber<CargoDropItemTouchOutedEvent> touchOutedSubscriber,
            ISubscriber<PlayerPickupInputEvent> pickupSubscriber)
        {
            touchedSubscriber.Subscribe(OnDropTouched);
            touchOutedSubscriber.Subscribe(OnDropTouchOuted);
            pickupSubscriber.Subscribe(OnPickup);
        }

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
            if (_cargoTower == null && _playerController != null)
            {
                _cargoTower = _playerController.CargoStackTower;
            }
        }

        private bool IsSelf(PlayerController player) => player == _playerController;

        private void OnDropTouched(CargoDropItemTouchedEvent evt)
        {
            if (!IsSelf(evt.Player)) return;
            if (evt.DropItem == null) return;

            _touchingItems.Add(evt.DropItem);
        }

        private void OnDropTouchOuted(CargoDropItemTouchOutedEvent evt)
        {
            if (!IsSelf(evt.Player)) return;
            if (evt.DropItem == null) return;

            _touchingItems.Remove(evt.DropItem);
        }

        private void OnPickup(PlayerPickupInputEvent evt)
        {
            if (!IsSelf(evt.Player)) return;
            if (_cargoTower == null) return;

            var dropItem = GetAnyTouchingItem();
            if (dropItem == null || dropItem.CargoView == null) return;

            var view = dropItem.CargoView;
            view.ChangeNormal(); // Preview状態を動的へ戻す
            _cargoTower.AttachExistingCargo(view);

            dropItem.OnPickedUp();
            _touchingItems.Remove(dropItem);
        }

        private CargoDropItemController GetAnyTouchingItem()
        {
            foreach (var item in _touchingItems)
            {
                return item;
            }

            return null;
        }
    }
}
