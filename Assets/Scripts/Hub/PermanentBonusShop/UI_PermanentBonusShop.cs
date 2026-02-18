using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PermanentBonusShop : UI_Menu
{
    [SerializeField] private List<UI_BuyPermanentBonusSlot> _buyPermanentBonusSlotList = new();

    [Header("Ticket")]
    [SerializeField] private Image _bonusIcon;
    [SerializeField] private TextMeshProUGUI _bonusName;
    [SerializeField] private TextMeshProUGUI _bonusDescription;

    [ReadOnly] private int _currentIndex;
    [ReadOnly] private BonusData _currentBonusData;
    [ReadOnly] private UI_BuyPermanentBonusSlot _currentBuyPermanentBonusSlot;

    public override void OpenMenu()
    {
        Setup();
        base.OpenMenu();
    }

    public override void CloseMenu()
    {
        base.CloseMenu();
    }

    public void Setup()
    {
        List<BonusData> bonusDataList = Resources.LoadAll<BonusData>("Bonus").ToList();
        bonusDataList = bonusDataList.FindAll(x => _currentIndex == (int)x.Category);

        for (int i = bonusDataList.Count - 1; i >= 0; i--)
        {
            for (int j = 0; j < bonusDataList[i].UpgradeBonusList.Count; j++)
            {
                bonusDataList.Remove(bonusDataList[i].UpgradeBonusList[j]);
            }
        }

        for (int i = 0; i < _buyPermanentBonusSlotList.Count; i++)
        {
            if (i < bonusDataList.Count)
            {
                _buyPermanentBonusSlotList[i].gameObject.SetActive(true);
                _buyPermanentBonusSlotList[i].Setup(bonusDataList[i]);
            }
            else
            {
                _buyPermanentBonusSlotList[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetupTicket(BonusData bonusData, UI_BuyPermanentBonusSlot permanentBonusShop)
    {
        _bonusIcon.sprite = bonusData.Icon;
        _bonusName.text = bonusData.Name;
        _bonusDescription.text = bonusData.Description;

        _currentBonusData = bonusData;
        _currentBuyPermanentBonusSlot = permanentBonusShop;
    }

    public void TicketButton()
    {
        if (_currentBuyPermanentBonusSlot.CurrentIndex - 1 >= _currentBuyPermanentBonusSlot.BonusData.UpgradeBonusList.Count) return;

        BonusData bonusData = _currentBuyPermanentBonusSlot.BonusData;
        if (_currentBuyPermanentBonusSlot.CurrentIndex > 0)
        {
            bonusData = _currentBuyPermanentBonusSlot.BonusData.UpgradeBonusList[_currentBuyPermanentBonusSlot.CurrentIndex - 1];
        }

        if (SaveManager.CurrentSave.PermanentBonusList.Contains(bonusData)) return;


        SaveManager.CurrentSave.PermanentBonusList.Add(bonusData);

        // increase buy permanent bonus slot current index 
        if (_currentBuyPermanentBonusSlot.CurrentIndex - 1 < _currentBuyPermanentBonusSlot.BonusData.UpgradeBonusList.Count - 1)
        {
            _currentBuyPermanentBonusSlot.CurrentIndex++;
        }

        //should show not the bonus we just unlocked but the one after
        if (_currentBuyPermanentBonusSlot.CurrentIndex - 1 < _currentBuyPermanentBonusSlot.BonusData.UpgradeBonusList.Count)
        {
            _currentBonusData = _currentBuyPermanentBonusSlot.BonusData.UpgradeBonusList[_currentBuyPermanentBonusSlot.CurrentIndex - 1];
        }

        Setup();
        SetupTicket(_currentBonusData, _currentBuyPermanentBonusSlot);
    }

    public void ChangeIndex(int newIndex)
    {
        _currentIndex = newIndex;
        Setup();
    }
}
