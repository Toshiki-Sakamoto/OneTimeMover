using Core.Common;
using Core.Common.Messaging;
using Core.Game;
using Core.Phase;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace OneTripMover.Views.Stage
{
    public class StageStartView : MonoBehaviour, MMEventListener<MMGameEvent>
    {
        [SerializeField] private MMF_Player _startFeedbacks;
        
        private IPhaseService<GamePhase> _gamePhaseService;


        public void Play()
        {
            _startFeedbacks?.PlayFeedbacks();
        }

        public void OnMMEvent(MMGameEvent eventType)
        {
            switch (eventType.EventName)
            {
                case "StartGame":
                    _gamePhaseService.ChangePhase(GamePhase.Play);
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
            _gamePhaseService = ServiceLocator.Resolve<IPhaseService<GamePhase>>();
        }
    }
}