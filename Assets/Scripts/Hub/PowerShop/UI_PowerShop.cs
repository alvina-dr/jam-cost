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
        for (int i = 0; i < _powerSlotList.Count; i++)
        {
            _powerSlotList[i].LockPowerSlot();
        }

        List<PowerData> powerDataList = Resources.LoadAll<PowerData>("Powers").ToList();

        for (int i = 0; i < powerDataList.Count; i++)
        {
            if (i >= powerDataList.Count) break;
            _buyPowerSlotList[i].SetupSlot(powerDataList[i]);
        }
    }

    public void SetupTicket(PowerData powerData)
    {
        if (powerData == null) return;
        _powerIcon.sprite = powerData.PowerSprite;
        _powerName.text = powerData.PowerName;
        _powerLore.text = powerData.PowerLore;
        _powerDescription.text = powerData.PowerDescription;
    }
}
