using UnityEngine;
using UnityEngine.UI;

namespace OneTripMover
{
    /// <summary>
    /// アンカーがゾーンに入ったらボタン表示を切り替える。
    /// </summary>
    public class AddOneZone : MonoBehaviour
    {
        public Rigidbody2D anchor;
        public StackTower stackTower;
        public Button addOneButton;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsAnchor(other))
            {
                SetButtonVisible(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (IsAnchor(other))
            {
                SetButtonVisible(false);
            }
        }

        private bool IsAnchor(Collider2D other)
        {
            return anchor != null && other.attachedRigidbody == anchor;
        }

        private void SetButtonVisible(bool visible)
        {
            if (addOneButton != null)
            {
                addOneButton.gameObject.SetActive(visible);
            }
        }
    }
}
