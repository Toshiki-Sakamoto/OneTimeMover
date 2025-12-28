using UnityEngine;

namespace UI.Money
{
    public class MoneyAnimationUIView : MonoBehaviour
    {
        [SerializeField] private Transform _root;
        [SerializeField] private GameObject _plusItemPrefab;
        [SerializeField] private GameObject _minusItemPrefab;

        public MoneyAnimationItemView SpawnItem(int delta)
        {
            if (_root == null) return null;

            var prefab = delta >= 0 ? _plusItemPrefab : _minusItemPrefab;
            if (prefab == null)
            {
                prefab = _plusItemPrefab ?? _minusItemPrefab;
            }
            if (prefab == null) return null;

            var go = Instantiate(prefab, _root);
            go.gameObject.SetActive(true);
            return go.GetComponent<MoneyAnimationItemView>();
        }
    }
}
