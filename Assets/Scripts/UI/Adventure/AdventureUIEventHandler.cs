using Core.Common;
using Core.Common.Messaging;

namespace UI.Adventure
{
    public class AdventureUIEventHandler : Core.IEventHandler
    {
        private readonly AdventureUIController _controller;

        public AdventureUIEventHandler()
        {
            _controller = ServiceLocator.Resolve<AdventureUIController>();

            var advanceSub = ServiceLocator.Resolve<ISubscriber<OneTripMover.Input.AdventureAdvanceInputEvent>>();
            advanceSub.Subscribe(_ => _controller.OnAdvance());

            var playSub = ServiceLocator.Resolve<ISubscriber<Core.Adventure.AdventurePlayEvent>>();
            playSub.Subscribe(_controller.OnPlayRequested);

            var playScriptSub = ServiceLocator.Resolve<ISubscriber<Core.Adventure.AdventurePlayScriptEvent>>();
            playScriptSub.Subscribe(_controller.OnPlayScriptRequested);
        }
    }
}
