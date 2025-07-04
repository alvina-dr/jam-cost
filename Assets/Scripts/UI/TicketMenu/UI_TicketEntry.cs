using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TicketEntry : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
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
        _itemIcon.sprite = data.Icon;
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        ItemBehavior itemBehavior = Instantiate(_data.Prefab);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        itemBehavior.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
        itemBehavior.StartDrag();
        transform.localScale = Vector3.zero;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.SelectedItem == null) return;
        GameManager.Instance.SelectedItem.EndDrag();
        GameManager.Instance.UIManager.TicketMenu.RemoveItemFromTicket(this);
    }

    public void OnDrag(PointerEventData eventData)
    {

    }
}
