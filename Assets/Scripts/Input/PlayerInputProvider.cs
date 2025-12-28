using Core.Common;
using Core.Common.Messaging;
using OneTripMover.Core.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OneTripMover.Input
{
    /// <summary>
    /// InputSystem_Actionsのイベントコールバック経由で入力をPublishする。
    /// </summary>
    public class PlayerInputProvider : MonoBehaviour
    {
        private IPublisher<PlayerInputEvent> _playerInputEventPublisher;
        private IPublisher<PlayerPickupInputEvent> _pickupInputEventPublisher;
        private InputSystem_Actions _actions;

        [Inject]
        public void Construct(
            IPublisher<PlayerInputEvent> inputPublisher,
            IPublisher<PlayerPickupInputEvent> pickupInputPublisher,
            InputSystem_Actions actions)
        {
            _playerInputEventPublisher = inputPublisher;
            _pickupInputEventPublisher = pickupInputPublisher;
            _actions = actions;

            OnEnable();
        }

        private void OnEnable()
        {
            if (_actions == null) return;
            _actions.Player.Balance.performed += OnBalance;
            _actions.Player.Balance.canceled += OnBalance;
            _actions.Player.Pick.performed += OnPick;

            _actions.Player.Enable();
        }

        private void OnDisable()
        {
            if (_actions == null) return;
            _actions.Player.Balance.performed -= OnBalance;
            _actions.Player.Balance.canceled -= OnBalance;
            _actions.Player.Pick.performed -= OnPick;

            _actions.Player.Disable();
        }

        private void Update()
        {
            if (_actions == null) return;
            
            var move = _actions.Player.Move.ReadValue<Vector2>();
            if (move != Vector2.zero)
            {
                _playerInputEventPublisher.Publish(new PlayerInputEvent(move, 0f));
            }
        }

        private void OnBalance(InputAction.CallbackContext ctx)
        {
            var balance = ctx.ReadValue<Vector2>().x;
            _playerInputEventPublisher.Publish(new PlayerInputEvent(Vector2.zero, balance));
        }

        private void OnPick(InputAction.CallbackContext ctx)
        {
            _pickupInputEventPublisher.Publish(new PlayerPickupInputEvent());
        }
    }
}
