using TMPro;
using UnityEngine;

namespace UI.Money
{
    public class MoneyUIView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _moneyText;

        public void SetAmount(int value)
        {
            _moneyText.text = $"{value}円";
        }
    }
}
