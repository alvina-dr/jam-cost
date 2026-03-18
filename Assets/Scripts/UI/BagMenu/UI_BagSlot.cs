using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_BagSlot : MonoBehaviour
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

    public void ClearSlot()
    {
        Destroy(CurrentBagItem?.gameObject);
        _currentBagItem = null;
        HidePrice();
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
