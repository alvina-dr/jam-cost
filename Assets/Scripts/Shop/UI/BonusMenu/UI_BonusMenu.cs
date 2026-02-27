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

    [Header("Reroll")]
    [SerializeField] private TextMeshProUGUI _rerollNumber;
    [SerializeField] private Transform _rerollParent;

    private BonusData _currentBonusData;
    private UI_BonusEntry _currentBonusEntry;

    private List<BonusData> _sellingBonusDataList = new();

    public override void OpenMenu()
    {
        for (int i = 0; i < _bonusEntryList.Count; i++)
        {
            _bonusEntryList[i].Button.interactable = true;
            _bonusEntryList[i].ButtonAnim.enabled = true;
        }

        Setup();

        base.OpenMenu();
    }

    public void Setup()
    {
        for (int i = 0; i < _bonusEntryList.Count; i++)
        {
            _bonusEntryList[i].SetupBonus(_sellingBonusDataList[i]);
        }

        _bonusName.gameObject.SetActive(false);
        _bonusDescription.gameObject.SetActive(false);

        _rerollNumber.text = SaveManager.CurrentSave.CurrentRun.Rerolls.ToString();

        if (SaveManager.CurrentSave.CurrentRun.Rerolls == 0) _rerollParent.gameObject.SetActive(false);
        else _rerollParent.gameObject.SetActive(true);
    }

    public void SelectBonusList()
    {
        if (_bonusEntryList.Count > DataLoader.Instance.RunBonusDataList.Count) Debug.LogWarning("Not enough bonus for this time");

        for (int i = 0; i < _bonusEntryList.Count; i++)
        {
            _sellingBonusDataList.Add(DataLoader.Instance.TakeRandomBonusData(BonusData.BonusDurability.Run, _sellingBonusDataList));
        }
    }

    public void ReleaseBonusList()
    {
        for (int i = 0; i < _bonusEntryList.Count; i++)
        {
            if (_sellingBonusDataList[i] != null)
            {
                DataLoader.Instance.RunBonusDataList.Add(_sellingBonusDataList[i]);
            }
        }

        _sellingBonusDataList.Clear();
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
    }

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

        for (int i = 0; i < _sellingBonusDataList.Count; i++)
        {
            if (_sellingBonusDataList[i] == _currentBonusData)
            {
                _sellingBonusDataList[i] = null;
            }
        }

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

    [Button]
    public void RerollShop()
    {
        if (SaveManager.CurrentSave.CurrentRun.Rerolls == 0) return;

        SaveManager.CurrentSave.CurrentRun.Rerolls--;

        ReleaseBonusList();
        SelectBonusList();
        OpenMenu();
    }
}
