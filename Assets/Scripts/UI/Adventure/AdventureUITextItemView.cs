using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace UI.Adventure
{
    /// <summary>
    /// アドベンチャー1行分の表示とタイプ演出を担当するビュー。
    /// </summary>
    public class AdventureUITextItemView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _charInterval = 0.1f;

        private Coroutine _routine;
        private string _fullText;
        private Action _onCompleted;

        public bool IsAnimating => _routine != null;

        public void Play(string text, Action onCompleted)
        {
            _fullText = text ?? string.Empty;
            _onCompleted = onCompleted;

            if (_text == null)
            {
                FinishImmediate();
                return;
            }

            _routine = StartCoroutine(TypeRoutine());
        }

        public void Skip()
        {
            if (_routine != null)
            {
                StopCoroutine(_routine);
                _routine = null;
            }

            if (_text != null)
            {
                _text.text = _fullText;
                _text.maxVisibleCharacters = int.MaxValue;
            }

            Complete();
        }

        public void Exit()
        {
            Destroy(gameObject);
        }

        private IEnumerator TypeRoutine()
        {
            // TMPのmaxVisibleCharactersを用いてリッチテキストを壊さずにタイプ表示
            _text.text = _fullText;
            _text.ForceMeshUpdate();
            var total = _text.textInfo.characterCount;
            _text.maxVisibleCharacters = 0;

            for (var i = 0; i <= total; i++)
            {
                _text.maxVisibleCharacters = i;
                yield return new WaitForSeconds(_charInterval);
            }

            _routine = null;
            Complete();
        }

        private void FinishImmediate()
        {
            if (_text != null)
            {
                _text.text = _fullText;
                _text.maxVisibleCharacters = int.MaxValue;
            }
            Complete();
        }

        private void Complete()
        {
            var cb = _onCompleted;
            _onCompleted = null;
            cb?.Invoke();
        }
    }
}
