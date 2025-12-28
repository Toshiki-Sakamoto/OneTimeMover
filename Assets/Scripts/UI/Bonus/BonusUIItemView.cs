using System;
using System.Collections;
using Febucci.TextAnimatorForUnity;
using Febucci.TextAnimatorForUnity.TextMeshPro;
using TMPro;
using UnityEngine;

namespace UI.Bonus
{
    [RequireComponent(typeof(TextAnimator_TMP))]
    public class BonusUIItemView : MonoBehaviour
    {
        [SerializeField] private string _showText;
//        [SerializeField] private TextMeshProUGUI _textMeshPro;
        private TextAnimator_TMP _textAnimator;
  //      [SerializeField] private TypewriterComponent _textAnimatorPlayer;
        [SerializeField] private float _duration = 1.0f;

        public IEnumerator PlayAndWait()
        {
            _textAnimator.SetText($"<rainb>{_showText}</rainb>");

            yield return new WaitForSeconds(_duration);
            Destroy(gameObject);
        }
        
        public void SetDefaultText()
        {
            _textAnimator.SetText(_showText);
        }

        private void Awake()
        {
            _textAnimator = GetComponent<TextAnimator_TMP>();
            SetDefaultText();
        }
    }
}
