using System.Threading.Tasks;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace UI.Bonus
{
    [RequireComponent(typeof(MMF_Player))]
    public class BonusUIItemView : MonoBehaviour
    {
        [SerializeField] private float _durationSeconds = 1.0f;
        private CanvasGroup _canvasGroup;
        private MMF_Player _player;
        private Vector3 _startPos;

        public async Task PlayAndWait(System.Action onCompleted = null)
        {
            if (_player != null)
            {
                _player.PlayFeedbacks();
            }
            
            var ms = Mathf.Max(0f, _durationSeconds) * 1000f;
            await Task.Delay((int)ms);
            onCompleted?.Invoke();
        }

        public void ResetPosition()
        {
            transform.localPosition = _startPos;
            _canvasGroup.alpha = 1f;
        }
        
        private void Awake()
        {
            _player = GetComponent<MMF_Player>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }
    }
}
