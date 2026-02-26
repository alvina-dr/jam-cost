using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_BagItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private UI_BagMenu _bagMenu;
    [SerializeField] private UI_BagSlot _formerBagSlot;
    [SerializeField] private UI_BagSlot _currentBagSlot;
    [SerializeField] private Image _image;
    [SerializeField] private ItemData _itemData;
    public ItemData Data => _itemData;

    private void Start()
    {
        _bagMenu = GameManager.Instance.UIManager.BagMenu;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("begin drag : " + name);
        _currentBagSlot.RemoveItem();
        _image.raycastTarget = false;
        transform.parent = _bagMenu.DraggedItem;
        _formerBagSlot = _currentBagSlot;
        _currentBagSlot = null;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _image.raycastTarget = true;
        if (_currentBagSlot == null)
        {
            _currentBagSlot = _formerBagSlot;
            transform.DOMove(_currentBagSlot.transform.position, .5f);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        transform.position = mouseWorldPos;
    }

    public void SetSlot(UI_BagSlot bagSlot)
    {
        _currentBagSlot = bagSlot;
        transform.parent = _currentBagSlot.BagItemParent;
    }

    public void Setup(ItemData itemData)
    {
        _itemData = itemData;
        _image.sprite = itemData.Icon;
    }
}
