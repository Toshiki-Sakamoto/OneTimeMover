using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace OneTripMover.Views.Stage
{
    public class TrackController : MonoBehaviour
    {
        [SerializeField] private MMF_Player _entryFeedbackPlayer;
        [SerializeField] private MMF_Player _exitFeedbackPlayer;
        
        public void PlayTrackFeedback()
        {
            _entryFeedbackPlayer.PlayFeedbacks();
        }
        
        public void PlayExitFeedback()
        {
            _exitFeedbackPlayer.PlayFeedbacks();
        }
        
        private void Awake()
        {
        }

    }
}