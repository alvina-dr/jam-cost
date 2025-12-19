using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BagSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private UI_OverCheck _overCheck;
    public bool Over => _overCheck.IsOver();
    [SerializeField] private UI_BagItem _bagItemPrefab;
    [SerializeField] private UI_BagItem _currentBagItem;
    public UI_BagItem CurrentBagItem => _currentBagItem;

    public void CreateItem(ItemData itemData)
    {
        UI_BagItem bagItem = Instantiate(_bagItemPrefab, transform.position, Quaternion.identity, transform);
        bagItem.Setup(itemData);
        bagItem.SetSlot(this);
        _currentBagItem = bagItem;
    }

    public void RemoveItem()
    {
        _currentBagItem = null;
    }

    public void ClearSlot()
    {
        Destroy(CurrentBagItem?.gameObject);
        _currentBagItem = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("DROPPED");

        if (_currentBagItem != null) return;

        GameObject dropped = eventData.pointerDrag;
        UI_BagItem bagItem = dropped.GetComponent<UI_BagItem>();

        if (bagItem != null )
        {
            _currentBagItem = bagItem;
            _currentBagItem.SetSlot(this);
            _currentBagItem.transform.DOMove(transform.position, .1f);
            RectTransform rect = (RectTransform)_currentBagItem.transform;
            rect.SetOffsets(0, 0, 0, 0);
        }
    }
}
