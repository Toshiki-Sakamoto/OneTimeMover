using System;
using Core.Common;
using Core.Common.Messaging;
using Core.Game;
using MoreMountains.Tools;
using UnityEngine;

namespace OneTripMover.Views.Stage
{
    public class StageIntro : MonoBehaviour, MMEventListener<MMGameEvent>
    {
        [SerializeField] private TrackController _trackController;
        private IPublisher<PlayerStatePrepareStartedEvent> _prepareStartedPublisher;
        private bool _isIntroFinished = false;

        public void PlayIntro()
        {
            if (_isIntroFinished) return;

            _trackController.PlayTrackFeedback();
            _isIntroFinished = true;
        }
        

        public void OnMMEvent(MMGameEvent eventType)
        {
            switch (eventType.EventName)
            {
                case "TrackEntryFinished":
                    _prepareStartedPublisher.Publish(new PlayerStatePrepareStartedEvent());
                    _trackController.PlayExitFeedback();
                    break;
                
                case "TrackExitFinished":
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
        }
    }
}