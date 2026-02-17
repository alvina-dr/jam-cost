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
        //List<BonusData> bonusUpgradeDataList = bonusDataList.FindAll(x => x.UpgradeBonusList.Count > 0);
        //bonusDataList = bonusDataList.FindAll(x => x.UpgradeBonusList.Count == 0);

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


        //for (int i = 0; i < bonusUpgradeDataList.Count; i++)
        //{
        //    UI_BuyPermanentBonusSlot parentBonusSlot = _buyPermanentBonusSlotList.Find(x => x.BonusData.Contains(bonusUpgradeDataList[i].UpgradeBonusList[0]));
        //    if (parentBonusSlot != null) parentBonusSlot.AddBonusUpgrade(bonusUpgradeDataList[i]);
        //}
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
        SaveManager.CurrentSave.PermanentBonusList.Add(_currentBonusData);
    }

    public void ChangeIndex(int newIndex)
    {
        _currentIndex = newIndex;
        Setup();
    }
}
