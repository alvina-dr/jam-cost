using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TicketEntry : MonoBehaviour
{
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemPrice;
    [SerializeField] private Image _starPrefab;
    [SerializeField] private Transform _starParent;

    public void Setup(ItemData data)
    {
        _itemIcon.sprite = data.ItemIcon;
        _itemName.text = data.Name;
        _itemPrice.text = data.Price.ToString() + "$";
        for (int i = 0; i < data.Rarity; i++)
        {
            Instantiate(_starPrefab, _starParent);
        }
    }
}
