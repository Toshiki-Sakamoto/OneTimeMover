using UnityEngine;
using UnityEngine.UI;

namespace OneTripMover
{
    /// <summary>
    /// ゴールエリアに一定時間留まれたらクリア。
    /// </summary>
    public class GoalZone : MonoBehaviour
    {
        public Rigidbody2D anchor;
        public float requiredHold = 2f;
        public Text statusLabel;

        private float _holdTimer;
        private bool _isInside;

        private void Update()
        {
            if (!_isInside)
            {
                return;
            }

            _holdTimer += Time.deltaTime;
            if (_holdTimer >= requiredHold)
            {
                SetStatus("お届け完了！");
                _isInside = false; // 一度だけ
            }
            else
            {
                SetStatus($"静止中... {(_holdTimer / requiredHold * 100f):F0}%");
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsAnchor(other))
            {
                _isInside = true;
                _holdTimer = 0f;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (IsAnchor(other))
            {
                _isInside = false;
                _holdTimer = 0f;
                SetStatus("玄関に入って停止してください");
            }
        }

        private bool IsAnchor(Collider2D other)
        {
            return anchor != null && other.attachedRigidbody == anchor;
        }

        private void SetStatus(string message)
        {
            if (statusLabel != null)
            {
                statusLabel.text = message;
            }
        }
    }
}
