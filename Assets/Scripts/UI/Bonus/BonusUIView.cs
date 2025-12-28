using TMPro;
using UnityEngine;

namespace UI.Bonus
{
    public class BonusUIView : MonoBehaviour
    {
        [SerializeField] private BonusUIItemView _perfectBonusText;
        [SerializeField] private BonusUIItemView _oneMoreBonusText;

        public BonusUIItemView PerfectBonusText => _perfectBonusText;
        public BonusUIItemView OneMoreBonusText => _oneMoreBonusText;
        
        public void SetPerfectActive(bool active)
        {
            if (_perfectBonusText != null)
            {
                _perfectBonusText.gameObject.SetActive(active);
            }
        }

        public void SetOneMoreActive(bool active)
        {
            if (_oneMoreBonusText != null)
            {
                _oneMoreBonusText.gameObject.SetActive(active);
            }
        }
    }
}
