using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PowerShop : UI_Menu
{
    [SerializeField] private List<UI_PowerSlot> _powerSlotList = new();
    [SerializeField] private List<UI_BuyPowerSlot> _buyPowerSlotList = new();

    [Header("Ticket")]
    [SerializeField] private Image _powerIcon;
    [SerializeField] private TextMeshProUGUI _powerName;
    [SerializeField] private TextMeshProUGUI _powerLore;
    [SerializeField] private TextMeshProUGUI _powerDescription;

    [Header("Equip / Buy Button")]
    [SerializeField] private TextMeshProUGUI _ticketButtonText;

    [SerializeField, ReadOnly] private UI_BuyPowerSlot _currentPowerSlot;
    [SerializeField, ReadOnly] private PowerData _currentPowerData;

    public override void OpenMenu()
    {
        Setup();
        base.OpenMenu();
    }


    public override void CloseMenu()
    {
        base.CloseMenu();
    }

    public void Setup(bool reset = true)
    {
        if (reset)
        {
            _currentPowerSlot = null;
            _currentPowerData = null;
        }

        for (int i = 0; i < _powerSlotList.Count; i++)
        {
            if (i + 1 <= SaveManager.CurrentSave.EquipedPowerMax)
            {
                if (i < SaveManager.CurrentSave.EquipedPowerDataList.Count) _powerSlotList[i].Setup(true, SaveManager.CurrentSave.EquipedPowerDataList[i]);
                else _powerSlotList[i].Setup(true);
            }
            else _powerSlotList[i].Setup();
        }

        for (int i = 0; i < _buyPowerSlotList.Count; i++)
        {
            _buyPowerSlotList[i].SetupSlot(i < DataLoader.Instance.PowerDataList.Count ? DataLoader.Instance.PowerDataList[i] : null);
        }

        if (reset) SetupTicket(DataLoader.Instance.PowerDataList[0], _buyPowerSlotList[0]);
    }

    public void SetupTicket(PowerData powerData, UI_BuyPowerSlot powerSlot)
    {
        if (powerData == null) return;

        _currentPowerData = powerData;
        _currentPowerSlot = powerSlot;
        _powerIcon.sprite = powerData.PowerSprite;
        _powerName.text = powerData.PowerName;
        _powerLore.text = powerData.PowerLore;
        _powerDescription.text = powerData.PowerDescription;

        //if the power is unlocked already
        if (SaveManager.CurrentSave.UnlockedPowerDataList.Contains(powerData))
        {
            if (SaveManager.CurrentSave.EquipedPowerDataList.Contains(_currentPowerData))
            {
                _ticketButtonText.text = "<wave amp=2>Unequip</wave>";
            }
            else
            {
                _ticketButtonText.text = "<wave amp=2>Equip</wave>";
            }
        }
        else
        {
            _ticketButtonText.text = "<wave amp=2>Buy</wave>";
        }
    }

    public void TicketButton()
    {
        if (_currentPowerData == null) return;

        // if unlocked
        if (SaveManager.CurrentSave.UnlockedPowerDataList.Contains(_currentPowerData))
        {
            // if equipped
            if (SaveManager.CurrentSave.EquipedPowerDataList.Contains(_currentPowerData))
            {
                UnequipPower();
            }
            else
            {
                EquipPower();
            }
        }
        else
        {
            BuyPower();
        }

        Setup(false);
        SetupTicket(_currentPowerData, _currentPowerSlot);
    }

    public void BuyPower()
    {
        if (SaveManager.CurrentSave.MealTickets < _currentPowerData.PowerPrice) return;

        SaveManager.Instance.AddMT(-_currentPowerData.PowerPrice);
        SaveManager.CurrentSave.UnlockedPowerDataList.Add(_currentPowerData);
    }

    public void EquipPower()
    {
        if (SaveManager.CurrentSave.EquipedPowerDataList.Count >= SaveManager.CurrentSave.EquipedPowerMax) return;

        SaveManager.CurrentSave.EquipedPowerDataList.Add(_currentPowerData);
        //_powerSlotList[SaveManager.CurrentSave.EquipedPowerDataList.Count - 1].SetPower(_currentPowerData);
    }

    public void UnequipPower()
    {
        //_powerSlotList[SaveManager.CurrentSave.EquipedPowerDataList.Count - 1].SetPower(_currentPowerData);
        SaveManager.CurrentSave.EquipedPowerDataList.Remove(_currentPowerData);
    }
}
