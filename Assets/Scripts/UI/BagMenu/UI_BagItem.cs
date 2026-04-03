using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BagItem : MonoBehaviour
{
    private UI_BagMenu _bagMenu;
    [SerializeField] private UI_BagSlot _formerBagSlot;
    [SerializeField] private UI_BagSlot _currentBagSlot;
    [SerializeField] private Image _image;
    [SerializeField] private ItemData _itemData;
    [SerializeField] private ParticleSystem _countItemParticle;
    public ItemData Data => _itemData;

    public int CurrentScore = 0;
    public List<int> CombinationItemAddList = new();
    public List<int> CombinationItemMultList = new();

    private void Start()
    {
        _bagMenu = GameManager.Instance.UIManager.BagMenu;
    }

    public void SetSlot(UI_BagSlot bagSlot)
    {
        _currentBagSlot = bagSlot;
        transform.SetParent(_currentBagSlot.BagItemParent);
    }

    public void Setup(ItemData itemData)
    {
        _itemData = itemData;
        _image.sprite = itemData.Icon;
        CurrentScore = _itemData.Price;
    }

    public void CountItem()
    {
        _countItemParticle.Play();
    }
}
