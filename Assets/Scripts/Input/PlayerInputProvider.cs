using System;
using Core.Common;
using Core.Common.Messaging;
using OneTripMover.Core.Player;
using OneTripMover.Views.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OneTripMover.Input
{
    /// <summary>
    /// プレイヤー入力処理
    /// </summary>
    public class PlayerInputProvider : MonoBehaviour
    {
        [SerializeField] private InputActionReference _moveAction;
        [SerializeField] private InputActionReference _balanceAction;
        [SerializeField] private InputActionReference _pickupAction;

        private IPublisher<PlayerInputEvent> _playerInputEventPublisher;
        private IPublisher<PlayerPickupInputEvent> _pickupInputEventPublisher;
        private IPlayerInputHandler _inputHandler;
        private PlayerController _playerController;
        private bool _inputEnabled = true;

        [Inject]
        public void Construct(
            IPublisher<PlayerInputEvent> inputPublisher,
            IPublisher<PlayerPickupInputEvent> pickupInputPublisher,
            ISubscriber<PlayerInputEnableEvent> inputEnableSubscriber)
        {
            _playerInputEventPublisher = inputPublisher;
            _pickupInputEventPublisher = pickupInputPublisher;
            inputEnableSubscriber.Subscribe(OnInputEnableChanged);
        }

        private void SetHandler(IPlayerInputHandler handler) =>
            _inputHandler = handler;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            _moveAction?.action.Enable();
            _balanceAction?.action.Enable();
            _pickupAction?.action.Enable();
        }

        private void Update()
        {
            if (!_inputEnabled) return;

            Vector2? moveInput = null;
            float? balanceInput = null;
            bool isAnyInput = false;
            var pickupPressed = _pickupAction != null && _pickupAction.action.triggered;
            
            if (_moveAction.action.IsPressed())
            {
                moveInput = _moveAction.action.ReadValue<Vector2>();
                isAnyInput = true;
            }
            
            if (_balanceAction != null && _balanceAction.action.triggered)
            {
                balanceInput = _balanceAction.action.ReadValue<Vector2>().x;
                isAnyInput = true;
            }

            if (isAnyInput)
            {
                _playerInputEventPublisher.Publish(new PlayerInputEvent(moveInput ?? Vector2.zero, balanceInput ?? 0f));
            }

            if (pickupPressed && _playerController != null)
            {
                _pickupInputEventPublisher.Publish(new PlayerPickupInputEvent
                {
                    Player = _playerController
                });
            }
        }

        private void OnInputEnableChanged(PlayerInputEnableEvent evt)
        {
            _inputEnabled = evt.Enabled;
        }
    }
}
