using Core.Common;
using Core.Stage;
using System.Collections;
using Core.Common.Messaging;

namespace UI.Bonus
{
    public class BonusUIController
    {
        private readonly BonusUIView _view;
        private readonly IStageUseCase _stageUseCase;
        private readonly IPublisher<BonusPresentationFinishedEvent> _finishedPublisher;

        public BonusUIController()
        {
            _view = ServiceLocator.Resolve<BonusUIView>();
            _stageUseCase = ServiceLocator.Resolve<IStageUseCase>();
            _finishedPublisher = ServiceLocator.Resolve<IPublisher<BonusPresentationFinishedEvent>>();
        }

        public void SetPerfect(bool active)
        {
            _view?.SetPerfectActive(active);
        }

        public void SetOneMore(bool active)
        {
            _view?.SetOneMoreActive(active);
        }

        public IEnumerator PlayBonusPresentation()
        {
            var hadBonus = false;
            var perfect = _stageUseCase.HasPerfectBonus();
            var oneMore = _stageUseCase.HasOneMoreBonus();

            if (perfect)
            {
                hadBonus = true;
                var item = _view.PerfectBonusText;
                if (item != null)
                {
                    yield return item.PlayAndWait();
                }
            }

            if (oneMore)
            {
                hadBonus = true;
                var item = _view.OneMoreBonusText;
                if (item != null)
                {
                    yield return item.PlayAndWait();
                }
            }

            _finishedPublisher?.Publish(new BonusPresentationFinishedEvent());
        }
    }
}
