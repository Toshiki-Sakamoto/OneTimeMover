using Core.Common;
using Core.Common.Messaging;
using Core.Game;
using Core.Phase;
using OneTripMover.Views.Player;
using UnityEngine;
using UnityEngine.Playables;

namespace OneTripMover.Views.Stage
{
    [RequireComponent(typeof(Collider2D))]
    public class GoalController : MonoBehaviour
    {
        [SerializeField] private PlayableDirector _director;
        [SerializeField] private Collider2D _collider;
 
        private IPublisher<GoalClearedEvent> _clearedPublisher;
        private IPublisher<GoalClearAnimationFinishedEvent> _clearAnimFinishedPublisher;
        private IPhaseService<GamePhase> _phaseService;
        private GoalResidentController _goalResident;
        private bool _cleared;
        
        public GoalResidentController GoalResident => _goalResident;

        public void SetGoalResident(GoalResidentController goalResident)
        {
            _goalResident = goalResident;
        }
        
        private void Awake()
        {
            _clearedPublisher = ServiceLocator.Resolve<IPublisher<GoalClearedEvent>>();
            _clearAnimFinishedPublisher = ServiceLocator.Resolve<IPublisher<GoalClearAnimationFinishedEvent>>();
            _phaseService = ServiceLocator.Resolve<IPhaseService<GamePhase>>();

            _collider ??= GetComponent<Collider2D>();
            if (_collider != null) _collider.isTrigger = true;
        }

        private void OnEnable()
        {
            if (_director != null)
            {
                _director.stopped += OnDirectorStopped;
            }
        }

        private void OnDisable()
        {
            if (_director != null)
            {
                _director.stopped -= OnDirectorStopped;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_cleared) return;

            var player = other.GetComponentInParent<PlayerController>();
            if (player == null) return;

            _cleared = true;
            if (_collider != null) _collider.enabled = false;

            _clearedPublisher?.Publish(new GoalClearedEvent { Player = player });
            _phaseService?.ChangePhase(GamePhase.Clear);
            
            _director.gameObject.SetActive(true);
            _director.Play();
        }

        /// <summary>
        /// Animation event hook.
        /// </summary>
        public void OnClearAnimationFinished()
        {
            _clearAnimFinishedPublisher?.Publish(new GoalClearAnimationFinishedEvent());
        }

        private void OnDirectorStopped(PlayableDirector director)
        {
            if (director == _director && _cleared)
            {
                OnClearAnimationFinished();
            }
        }
    }
}
