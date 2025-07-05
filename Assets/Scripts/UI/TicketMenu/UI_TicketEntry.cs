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
    [SerializeField] private Transform _horizontalLayout;

    [SerializeField] private Image _raycastImage;

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
        _horizontalLayout.DOScale(1.3f, .1f).SetUpdate(true);
        _itemPrice.transform.DOScale(1.8f, .1f).SetUpdate(true).OnComplete(() =>
        {
            _itemPrice.transform.DOScale(1f, .1f);
            _horizontalLayout.DOScale(1f, .1f).SetUpdate(true);
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

    private void Update()
    {
        if (GameManager.Instance.SelectedItem != null) _raycastImage.raycastTarget = false;
        else _raycastImage.raycastTarget = true;
    }

    public void OnDrag(PointerEventData eventData)
    {

    }
}
