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

        private AdventureUITextItemView _currentItem;
        private AdventureUITextItemView _previousItem;
        private Action _onCompleted;

        public string Key => _key;
        public bool IsAnimating => _currentItem != null && _currentItem.IsAnimating;

        public void ShowLine(string text, Action onCompleted)
        {
            gameObject.SetActive(true);
            
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

        public void ExitCurrentItem()
        {
            if (_currentItem != null)
            {
                _currentItem.Exit();
                _previousItem = _currentItem;
                _currentItem = null;
            }
            else if (_previousItem != null)
            {
                _previousItem.Exit();
                _previousItem = null;
            }
        }

        public void ClearView()
        {
            if (_entriesRoot == null) return;
            for (var i = _entriesRoot.childCount - 1; i >= 0; i--)
            {
                Destroy(_entriesRoot.GetChild(i).gameObject);
            }
            _currentItem = null;
            _previousItem = null;
            gameObject.SetActive(false);
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
                    _previousItem = item;
                }
                else
                {
                    var uiText = go.GetComponentInChildren<Text>();
                    if (uiText != null) uiText.text = text;
                }
            }
            Complete();
        }

        public void Complete()
        {
            gameObject.SetActive(false);
        }


        private void OnItemCompleted()
        {
        }

        private void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}
