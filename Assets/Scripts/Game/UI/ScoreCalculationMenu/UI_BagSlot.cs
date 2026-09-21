using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_BagSlot : MonoBehaviour
{
    [SerializeField] private UI_OverCheck _overCheck;
    public bool Over => _overCheck.IsOver();
    [SerializeField] private Item_ScoreCalculation _bagItemPrefab;
    [SerializeField] private Item_ScoreCalculation _currentBagItem;
    public Item_ScoreCalculation CurrentBagItem => _currentBagItem;

    [SerializeField] private UI_TextValue _priceText;
    [SerializeField] private GameObject _priceGO;
    public Transform BagItemParent;

    public void CreateItem(ItemInstance itemData)
    {
        Item_ScoreCalculation bagItem = Instantiate(_bagItemPrefab, transform.position, Quaternion.identity, BagItemParent);
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

    public void SetPriceTextNumber(int oldPrice, int newPrice)
    {
        _priceText.SetTextValueNumber(oldPrice, newPrice, .4f);
    }

    public void HidePrice()
    {
        _priceText.SetTextValue(string.Empty, false);
        _priceGO.gameObject.SetActive(false);
    }
}
