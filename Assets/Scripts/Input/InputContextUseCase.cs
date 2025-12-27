using Core.Common;
using UnityEngine.InputSystem;

namespace OneTripMover.Input
{
    public interface IInputContextUseCase
    {
        void SetPlayerContext();
        void SetUIContext();
    }

    public class InputContextUseCase : IInputContextUseCase
    {
        private readonly InputSystem_Actions _actions;

        public InputContextUseCase()
        {
            _actions = ServiceLocator.Resolve<InputSystem_Actions>();
            SetPlayerContext();
        }

        public void SetPlayerContext()
        {
            _actions.UI.Disable();
            _actions.Player.Enable();
        }

        public void SetUIContext()
        {
            _actions.Player.Disable();
            _actions.UI.Enable();
        }
    }
}
