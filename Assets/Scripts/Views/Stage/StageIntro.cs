using Core.Adventure;
using Core.Common;
using Core.Common.Messaging;
using Core.Stage;
using Core.Game;
using MoreMountains.Tools;
using OneTripMover.Master;
using UnityEngine;

namespace OneTripMover.Views.Stage
{
    public class StageIntro : MonoBehaviour, MMEventListener<MMGameEvent>
    {
        [SerializeField] private TrackController _trackController;
        [SerializeField] private StageStartView _stageStartView;
        [SerializeField] private string _introViewKey = "default";
        
        private IPublisher<PlayerStatePrepareStartedEvent> _prepareStartedPublisher;
        private IPublisher<AdventurePlayScriptEvent> _playScriptPublisher;
        private IStageMasterRegistry _stageMasterRegistry;
        private IStageRepository _stageRepository;
        private bool _isIntroFinished;
        private bool _isWaitingAdventure;

        public void PlayIntro()
        {
            if (_isIntroFinished) return;
            _trackController.PlayTrackFeedback();
            _isIntroFinished = true;
        }

        public void NextPlayerPrepare()
        {
            if (_isWaitingAdventure) return;
            _prepareStartedPublisher.Publish(new PlayerStatePrepareStartedEvent());
            _trackController.PlayExitFeedback();
        }

        public void OnMMEvent(MMGameEvent eventType)
        {
            switch (eventType.EventName)
            {
                case "TrackEntryFinished":
                    TryPlayIntroAdventureOrNext();
                    break;
                case "TrackExitFinished":
                    _stageStartView.Play();
                    break;
            }
        }
        
        private void OnEnable()
        {
            this.MMEventStartListening<MMGameEvent>();
        }
        
        private void OnDisable()
        {
            this.MMEventStopListening<MMGameEvent>();
        }
        
        public void Awake()
        {
            _prepareStartedPublisher = ServiceLocator.Resolve<IPublisher<PlayerStatePrepareStartedEvent>>();
            _playScriptPublisher = ServiceLocator.Resolve<IPublisher<AdventurePlayScriptEvent>>();
            _stageMasterRegistry = ServiceLocator.Resolve<IStageMasterRegistry>();
            _stageRepository = ServiceLocator.Resolve<IStageRepository>();
        }

        private void TryPlayIntroAdventureOrNext()
        {
            if (_isWaitingAdventure) return;
            if (_stageMasterRegistry.TryGetByStageId(_stageRepository.GetCurrentStageId(), out var master)
                && master.IntroAdventure != null)
            {
                _isWaitingAdventure = true;
                _playScriptPublisher.Publish(new AdventurePlayScriptEvent
                {
                    Script = master.IntroAdventure,
                    ViewKey = _introViewKey,
                    OnFinished = OnAdventureFinished
                });
            }
            else
            {
                NextPlayerPrepare();
            }
        }

        private void OnAdventureFinished()
        {
            _isWaitingAdventure = false;
            NextPlayerPrepare();
        }
    }
}
