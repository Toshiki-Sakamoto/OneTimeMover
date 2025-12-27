using Core.Common;
using Core.Common.Messaging;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OneTripMover.Input
{
    /// <summary>
    /// アドベンチャーUI用入力（UIアクションマップのClickを利用）。
    /// </summary>
    public class AdventureInputProvider : MonoBehaviour
    {
        private IPublisher<AdventureAdvanceInputEvent> _publisher;
        private InputSystem_Actions _actions;

        [Inject]
        public void Construct(
            IPublisher<AdventureAdvanceInputEvent> publisher,
            InputSystem_Actions actions)
        {
            _publisher = publisher;
            _actions = actions;
        }

        private void OnEnable()
        {
            if (_actions == null) return;
            _actions.UI.Click.performed += OnClick;
        }

        private void OnDisable()
        {
            if (_actions == null) return;
            _actions.UI.Click.performed -= OnClick;
        }

        private void OnClick(InputAction.CallbackContext ctx)
        {
            _publisher?.Publish(new AdventureAdvanceInputEvent());
        }
    }
}
