using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TicketEntry : MonoBehaviour
{
    [SerializeField] private ItemData _data;
    public ItemData Data => _data;

    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemPrice;
    [SerializeField] private Image _starPrefab;
    [SerializeField] private Transform _starParent;

    public void Setup(ItemData data)
    {
        _data = data;
        _itemIcon.sprite = data.ItemIcon;
        _itemName.text = data.Name;
        _itemPrice.text = data.Price.ToString() + "$";
        for (int i = 0; i < data.Rarity; i++)
        {
            Instantiate(_starPrefab, _starParent);
        }
    }

    public void BumpPrice()
    {
        _itemPrice.transform.DOScale(1.8f, .1f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
        {
            _itemPrice.transform.DOScale(1f, .1f);
        });
    }
}
