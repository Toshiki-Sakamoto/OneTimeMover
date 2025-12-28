using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Views.Cargo
{
    /// <summary>
    /// 荷物の傾きに応じて左右の警告UIを点灯・アニメーションするビュー。
    /// 傾きが10度未満なら非表示、しきい値に応じて色とアニメ速度を変更する。
    /// </summary>
    public class CargoDangerIndicatorView : MonoBehaviour
    {
        [Serializable]
        public class DangerLevel
        {
            public float thresholdDeg = 10f;
            public Color color = Color.white;
            public float moveSpeed = 1f;
        }

        [SerializeField] private RectTransform _leftIndicator;
        [SerializeField] private RectTransform _rightIndicator;
        [SerializeField] private TMP_Text _leftText;
        [SerializeField] private TMP_Text _rightText;
        [SerializeField] private float _baseTiltDeg = 20f;
        [SerializeField] private float _moveAmplitude = 10f;
        [SerializeField] private float _thresholdDeg = 10f;
        [SerializeField] private List<DangerLevel> _levels = new();

        private Vector3 _leftBasePos;
        private Vector3 _rightBasePos;
        private float _currentAngleAbs;
        private float _currentLimit;
        private int _currentSide; // -1 left, 1 right, 0 none

        private void Awake()
        {
            if (_leftIndicator != null) _leftBasePos = _leftIndicator.anchoredPosition3D;
            if (_rightIndicator != null) _rightBasePos = _rightIndicator.anchoredPosition3D;
            HideAll();
        }

        /// <summary>
        /// 荷物の傾き角度を渡す（+右、-左）。角度に応じて表示・アニメ更新。
        /// </summary>
        public void SetAngle(float limitAngleDeg, float currentAngleDeg)
        {
            _currentLimit = limitAngleDeg;
            _currentAngleAbs = Mathf.Abs(currentAngleDeg);
            _currentSide = _currentAngleAbs < _thresholdDeg ? 0 : (currentAngleDeg > 0 ? 1 : -1);
            UpdateVisual();
        }

        private void Update()
        {
            if (_currentSide == 0) return;
            AnimateIndicator();
        }

        private void UpdateVisual()
        {
            if (_currentSide == 0)
            {
                HideAll();
                return;
            }

            var level = ResolveLevel(_currentLimit, _currentAngleAbs);
            var target = _currentSide > 0 ? _rightIndicator : _leftIndicator;
            var text = _currentSide > 0 ? _rightText : _leftText;

            if (target == null) return;
            target.gameObject.SetActive(true);
            if (text != null) text.gameObject.SetActive(true);

            var c = level.color;
            SetColor(_currentSide > 0 ? _rightText : _leftText, c);
            SetColor(_currentSide > 0 ? _leftText : _rightText, Color.clear);
        }

        private DangerLevel ResolveLevel(float limitAngle, float angleAbs)
        {
            DangerLevel result = null;
            foreach (var lvl in _levels)
            {
                if (angleAbs >= Mathf.Max(0f, limitAngle - lvl.thresholdDeg))
                {
                    if (result == null || lvl.thresholdDeg < result.thresholdDeg)
                    {
                        result = lvl;
                    }
                }
            }
            return result ?? new DangerLevel { thresholdDeg = 10f, color = Color.white, moveSpeed = 1f };
        }

        private void AnimateIndicator()
        {
            var level = ResolveLevel(_currentLimit, _currentAngleAbs);
            var dirDeg = _currentSide > 0 ? _baseTiltDeg : -_baseTiltDeg;
            var dir = Quaternion.Euler(0f, 0f, dirDeg) * Vector2.right;
            var offset = dir.normalized * (_moveAmplitude * Mathf.Sin(Time.time * level.moveSpeed));

            if (_currentSide > 0 && _rightIndicator != null)
            {
                _rightIndicator.anchoredPosition3D = _rightBasePos + (Vector3)offset;
            }
            else if (_currentSide < 0 && _leftIndicator != null)
            {
                _leftIndicator.anchoredPosition3D = _leftBasePos + (Vector3)offset;
            }
        }

        private void HideAll()
        {
            if (_leftIndicator != null) _leftIndicator.gameObject.SetActive(false);
            if (_rightIndicator != null) _rightIndicator.gameObject.SetActive(false);
            if (_leftText != null) _leftText.gameObject.SetActive(false);
            if (_rightText != null) _rightText.gameObject.SetActive(false);
        }

        private void SetColor(TMP_Text text, Color color)
        {
            if (text == null) return;
            text.color = color;
        }
    }
}
