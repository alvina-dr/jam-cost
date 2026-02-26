using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BagSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private UI_OverCheck _overCheck;
    public bool Over => _overCheck.IsOver();
    [SerializeField] private UI_BagItem _bagItemPrefab;
    [SerializeField] private UI_BagItem _currentBagItem;
    public UI_BagItem CurrentBagItem => _currentBagItem;

    [SerializeField] private UI_TextValue _priceText;
    [SerializeField] private GameObject _priceGO;
    public Transform BagItemParent;

    public void CreateItem(ItemData itemData)
    {
        UI_BagItem bagItem = Instantiate(_bagItemPrefab, transform.position, Quaternion.identity, BagItemParent);
        bagItem.Setup(itemData);
        bagItem.SetSlot(this);
        _currentBagItem = bagItem;
        HidePrice();
    }

    public void RemoveItem()
    {
        _currentBagItem = null;
        HidePrice();
    }

    public void ClearSlot()
    {
        Destroy(CurrentBagItem?.gameObject);
        _currentBagItem = null;
        HidePrice();
    }

    public void OnDrop(PointerEventData eventData)
    {
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

    public void SetPriceText(int number)
    {
        _priceText.SetTextValue($"<wave amp=1>{number}");
        _priceGO.gameObject.SetActive(true);
    }

    public void HidePrice()
    {
        _priceText.SetTextValue(string.Empty, false);
        _priceGO.gameObject.SetActive(false);
    }
}
