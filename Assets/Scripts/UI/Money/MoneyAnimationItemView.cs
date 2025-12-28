using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;

namespace UI.Money
{
    public class MoneyAnimationItemView : MonoBehaviour, MMEventListener<MMGameEvent>
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private MMF_Player _player;

        public event Action ApplyRequested;

        private void Awake()
        {
            if (_player != null && _player.Events != null)
            {
                _player.Events.OnComplete.AddListener(OnComplete);
            }
            
            this.MMEventStartListening<MMGameEvent>();
        }

        public void Play(int delta)
        {
            if (_text != null)
            {
                _text.text = (delta >= 0 ? "+" : string.Empty) + delta.ToString() + "円";
            }

            _player?.PlayFeedbacks();
        }

        /// <summary>
        /// 演出中に呼び出すフック。MMF から UnityEvent 経由で呼んでください。
        /// </summary>
        public void OnApplyMoment()
        {
            ApplyRequested?.Invoke();
        }

        private void OnComplete()
        {
            OnApplyMoment();
            Destroy(gameObject);
        }

        public void OnMMEvent(MMGameEvent eventType)
        {
            if (eventType.EventName == "MoneyChanged")
            {
            }
        }
    }
}
