using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_BonusMenu : UI_Menu
{
    public List<UI_BonusEntry> _bonusEntryList = new();

    [SerializeField] private TextMeshProUGUI _bonusName;
    [SerializeField] private TextMeshProUGUI _bonusDescription;
    [SerializeField] private TextMeshProUGUI _bonusPrice;

    private BonusData _currentBonusData;
    private UI_BonusEntry _currentBonusEntry;

    public override void OpenMenu()
    {
        if (_bonusEntryList.Count > DataLoader.Instance.BonusDataList.Count) Debug.LogError("Not enough bonus for this time");

        for (int i = 0; i < _bonusEntryList.Count; i++)
        {
            _bonusEntryList[i].Button.interactable = true;
            _bonusEntryList[i].ButtonAnim.enabled = true;
        }

        for (int i = 0; i < _bonusEntryList.Count; i++)
        {
            _bonusEntryList[i].SetupBonus(DataLoader.Instance.TakeRandomBonusData());
        }

        _bonusName.gameObject.SetActive(false);
        _bonusDescription.gameObject.SetActive(false);
        //_bonusDescription.gameObject.SetActive(false);

        base.OpenMenu();
    }

    public override void CloseMenu()
    {
        for (int i = 0; i < _bonusEntryList.Count; i++)
        {
            _bonusEntryList[i].Button.interactable = false;
            _bonusEntryList[i].ButtonAnim.enabled = false;
            _bonusEntryList[i].transform.DOKill();
        }
        
        base.CloseMenu();

        for (int i = 0; i < _bonusEntryList.Count; i++)
        {
            if (_bonusEntryList[i].BonusData != null)
                DataLoader.Instance.BonusDataList.Add(_bonusEntryList[i].BonusData);
        }
    }

    //public override void Modif_OnCancel(InputAction.CallbackContext context)
    //{
    //    base.Modif_OnCancel(context);
    //}

    public void SetupInfo(BonusData bonusData, UI_BonusEntry bonusEntry)
    {
        _bonusName.gameObject.SetActive(true);
        _bonusDescription.gameObject.SetActive(true);
        _currentBonusData = bonusData;
        _currentBonusEntry = bonusEntry;
        _bonusName.text = _currentBonusData.Name;
        _bonusDescription.text = _currentBonusData.Description;
        _bonusPrice.text = $"Buy ({_currentBonusData.Price} <sprite name=PP>)";
    }

    public void Button()
    {
        if (_currentBonusData == null) return;

        if (_currentBonusData.Price > SaveManager.CurrentSave.CurrentRun.ProductivityPoints) return;

        _currentBonusData.GetBonus();
        SaveManager.Instance.AddPP(-_currentBonusData.Price);
        _currentBonusData = null;
        _currentBonusEntry.SetupBonus(null);
    }

    [Button]
    public void UpdateBonusEntryList()
    {
        _bonusEntryList.Clear();
        _bonusEntryList = GetComponentsInChildren<UI_BonusEntry>().ToList();
    }
}
