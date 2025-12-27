using System;
using Core.Common;
using Core.Common.Messaging;
using MoreMountains.Feedbacks;
using OneTripMover.Views.Player;
using UnityEngine;

namespace Views.Cargo
{
    [DefaultExecutionOrder(-50)]
    public class CargoDropItemController : MonoBehaviour
    {
        [SerializeField] private CargoView _cargoView;
        [SerializeField] private Collider2D _collider2d;
        [SerializeField] private Transform _triggerdRoot;
        [SerializeField] private Transform _notTriggeredRoot;
        [SerializeField] private Transform _cargoRoot;
        [SerializeField] private MMF_Player _pickupFeedback;
        
        private IPublisher<CargoDropItemTouchedEvent> _touchedPublisher;
        private IPublisher<CargoDropItemTouchOutedEvent> _touchOutedPublisher;

        public CargoView CargoView => _cargoView;

        private void ChangeState(bool triggered)
        {
            if (_notTriggeredRoot != null) _notTriggeredRoot.gameObject.SetActive(!triggered);
            if (_triggerdRoot != null) _triggerdRoot.gameObject.SetActive(triggered);
        }
        
        private void Awake()
        {
            ChangeState(false);
            
            _touchedPublisher = ServiceLocator.Resolve<IPublisher<CargoDropItemTouchedEvent>>();
            _touchOutedPublisher = ServiceLocator.Resolve<IPublisher<CargoDropItemTouchOutedEvent>>();
            
            _collider2d ??= GetComponent<Collider2D>();
            if (_collider2d != null)
            {
                _collider2d.isTrigger = true;
            }
            
            if (_cargoView == null && _cargoRoot != null)
            {
                _cargoView = _cargoRoot.GetComponentInChildren<CargoView>();
            }

            if (_cargoView != null)
            {
                _cargoView.ChangePreview();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.GetComponentInParent<PlayerController>();
            if (player == null) return;

            ChangeState(true);
            
            _touchedPublisher.Publish(new CargoDropItemTouchedEvent
            {
                DropItem = this,
                CargoView = _cargoView,
                Player = player,
                PlayerCollider = other,
            });
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var player = other.GetComponentInParent<PlayerController>();
            if (player == null) return;

            ChangeState(false);
            
            _touchOutedPublisher.Publish(new CargoDropItemTouchOutedEvent
            {
                DropItem = this,
                CargoView = _cargoView,
                Player = player,
                PlayerCollider = other,
            });
        }

        public void OnPickedUp()
        {
            ChangeState(false);
            if (_collider2d != null)
            {
                _collider2d.enabled = false;
            }

            enabled = false;
            
            _pickupFeedback.PlayFeedbacks();
        }
    }
}
