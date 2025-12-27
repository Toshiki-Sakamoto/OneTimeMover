using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Adventure
{
    public class AdventureUIView : MonoBehaviour, IAdventureUIView
    {
        [SerializeField] private string _key = "default";
        [SerializeField] private Transform _entriesRoot;
        [SerializeField] private GameObject _entryPrefab;
        [SerializeField] private float _exitDelay = 0.2f;

        private AdventureUITextItemView _currentItem;
        private Action _onCompleted;
        private Coroutine _exitRoutine;

        public string Key => _key;
        public bool IsAnimating => _currentItem != null && _currentItem.IsAnimating;

        public void ShowLine(string text, Action onCompleted)
        {
            _onCompleted = onCompleted;
            if (_entriesRoot == null || _entryPrefab == null)
            {
                FinishImmediate(text);
                return;
            }

            var go = Instantiate(_entryPrefab, _entriesRoot);
            _currentItem = go.GetComponentInChildren<AdventureUITextItemView>();
            if (_currentItem == null)
            {
                FinishImmediate(text);
                return;
            }

            _currentItem.Play(text, OnItemCompleted);
        }

        public void SkipAnimation()
        {
            _currentItem?.Skip();
        }

        public void ClearView()
        {
            if (_entriesRoot == null) return;
            for (var i = _entriesRoot.childCount - 1; i >= 0; i--)
            {
                Destroy(_entriesRoot.GetChild(i).gameObject);
            }
        }

        private void FinishImmediate(string text)
        {
            if (_entriesRoot != null && _entryPrefab != null)
            {
                var go = Instantiate(_entryPrefab, _entriesRoot);
                var item = go.GetComponentInChildren<AdventureUITextItemView>();
                if (item != null)
                {
                    item.Play(text, null);
                    item.Skip();
                }
                else
                {
                    var uiText = go.GetComponentInChildren<Text>();
                    if (uiText != null) uiText.text = text;
                }
            }
            Complete();
        }

        private void Complete()
        {
            if (_exitRoutine != null)
            {
                StopCoroutine(_exitRoutine);
            }
            _exitRoutine = StartCoroutine(ExitDelayRoutine());
        }

        private IEnumerator ExitDelayRoutine()
        {
            yield return new WaitForSeconds(_exitDelay);
            var callback = _onCompleted;
            _onCompleted = null;
            _currentItem = null;
            _exitRoutine = null;
            callback?.Invoke();
        }

        private void OnItemCompleted()
        {
            Complete();
        }
    }
}
