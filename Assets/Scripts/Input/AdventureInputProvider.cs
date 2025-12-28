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
        private IPublisher<AdventureSkipInputEvent> _skipPublisher;
        private InputSystem_Actions _actions;

        [Inject]
        public void Construct(
            IPublisher<AdventureAdvanceInputEvent> publisher,
            IPublisher<AdventureSkipInputEvent> skipPublisher,
            InputSystem_Actions actions)
        {
            _publisher = publisher;
            _skipPublisher = skipPublisher;
            _actions = actions;

            OnEnable();
        }

        private void OnEnable()
        {
            if (_actions == null) return;
            _actions.UI.Click.performed += OnClick;
            _actions.UI.Skip.performed += OnSkip;
        }

        private void OnDisable()
        {
            if (_actions == null) return;
            _actions.UI.Click.performed -= OnClick;
            _actions.UI.Skip.performed -= OnSkip;
        }

        private void OnClick(InputAction.CallbackContext ctx)
        {
            _publisher?.Publish(new AdventureAdvanceInputEvent());
        }

        private void OnSkip(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;
            _skipPublisher?.Publish(new AdventureSkipInputEvent());
        }
    }
}
