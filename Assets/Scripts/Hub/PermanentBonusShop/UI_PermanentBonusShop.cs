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
    [SerializeField] private TextMeshProUGUI _ticketButtonText;

    [ReadOnly] private int _currentIndex;
    [ReadOnly] private BonusData _currentBonusData;
    [ReadOnly] private UI_BuyPermanentBonusSlot _currentBuyPermanentBonusSlot;

    public override void OpenMenu()
    {
        ChangeIndex(0);
        base.OpenMenu();
    }

    public override void CloseMenu()
    {
        base.CloseMenu();
    }

    public void Setup()
    {
        List<BonusData> bonusDataList = Resources.LoadAll<BonusData>("Bonus").ToList();
        bonusDataList = bonusDataList.FindAll(x => x.Durability == BonusData.BonusDurability.Permanent);
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

    public void SetupTicket(BonusData bonusData, UI_BuyPermanentBonusSlot permanentBonusSlot)
    {
        if (bonusData == null)
        {
            _bonusIcon.sprite = null;
            _bonusIcon.gameObject.SetActive(false);
            _bonusName.text = string.Empty;
            _bonusDescription.text = string.Empty;
            _ticketButtonText.text = string.Empty;
            _currentBonusData = null;
            _currentBuyPermanentBonusSlot = null;

            return;
        }

        _bonusIcon.gameObject.SetActive(true);
        _bonusIcon.sprite = bonusData.Icon;
        _bonusName.text = bonusData.Name;
        _bonusDescription.text = bonusData.Description;

        _ticketButtonText.text = $"Buy ({bonusData.Price}<sprite name=MT>)";

        if (bonusData.UpgradeBonusList.Count == 0) 
        {
            if (SaveManager.CurrentSave.PermanentBonusList.Contains(bonusData))
            {
                _ticketButtonText.text = $"Complete";
            }
        }
        else
        {
            if (permanentBonusSlot.CurrentIndex >= bonusData.UpgradeBonusList.Count || SaveManager.CurrentSave.PermanentBonusList.Contains(bonusData.UpgradeBonusList[permanentBonusSlot.CurrentIndex]))
            {
                _ticketButtonText.text = $"Complete";
            }
        }

        _currentBonusData = bonusData;
        _currentBuyPermanentBonusSlot = permanentBonusSlot;
    }

    public void TicketButton()
    {
        if (_currentBonusData == null) return;
        if (_currentBuyPermanentBonusSlot == null) return;

        if (_currentBuyPermanentBonusSlot.CurrentIndex - 1 >= _currentBuyPermanentBonusSlot.BonusData.UpgradeBonusList.Count) return;

        BonusData bonusData = _currentBuyPermanentBonusSlot.BonusData;
        if (_currentBuyPermanentBonusSlot.CurrentIndex > 0)
        {
            bonusData = _currentBuyPermanentBonusSlot.BonusData.UpgradeBonusList[_currentBuyPermanentBonusSlot.CurrentIndex - 1];
        }

        if (SaveManager.CurrentSave.PermanentBonusList.Contains(bonusData)) return;


        // check if can buy
        if (SaveManager.CurrentSave.MealTickets < bonusData.Price) return;
        
        SaveManager.CurrentSave.PermanentBonusList.Add(bonusData);
        bonusData.GetBonus();
        SaveManager.Instance.AddMT(-bonusData.Price);

        // increase buy permanent bonus slot current index 
        if (_currentBuyPermanentBonusSlot.BonusData.UpgradeBonusList.Count > 0 &&
            _currentBuyPermanentBonusSlot.CurrentIndex - 1 < _currentBuyPermanentBonusSlot.BonusData.UpgradeBonusList.Count - 1)
        {
            _currentBuyPermanentBonusSlot.CurrentIndex++;
        }

        //should show not the bonus we just unlocked but the one after
        if (_currentBuyPermanentBonusSlot.BonusData.UpgradeBonusList.Count > 0 &&
            _currentBuyPermanentBonusSlot.CurrentIndex - 1 < _currentBuyPermanentBonusSlot.BonusData.UpgradeBonusList.Count)
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

        if (_buyPermanentBonusSlotList[0].gameObject.activeSelf)
        {
            BonusData correctBonusData = _buyPermanentBonusSlotList[0].BonusData;

            // if first level bonus data is already known
            if (correctBonusData.UpgradeBonusList.Count > 0 &&
                SaveManager.CurrentSave.PermanentBonusList.Contains(correctBonusData))
            {
                if (_buyPermanentBonusSlotList[0].CurrentIndex - 1 < correctBonusData.UpgradeBonusList.Count)
                    correctBonusData = correctBonusData.UpgradeBonusList[_buyPermanentBonusSlotList[0].CurrentIndex - 1];
                else
                    correctBonusData = correctBonusData.UpgradeBonusList.Last();
            }

            SetupTicket(correctBonusData, _buyPermanentBonusSlotList[0]);
        }
        else
        {
            SetupTicket(null, null);
        }
    }
}
