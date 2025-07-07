using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_BonusMenu : MonoBehaviour
{
    public UI_Menu Menu;
    public List<UI_BonusEntry> _bonusEntryList = new();

    public void OpenMenu()
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
        Menu.OpenMenu();
    }

    public void CloseMenu()
    {
        for (int i = 0; i < _bonusEntryList.Count; i++)
        {
            _bonusEntryList[i].Button.interactable = false;
            _bonusEntryList[i].ButtonAnim.enabled = false;
            _bonusEntryList[i].transform.DOKill();
        }
        Menu.CloseMenu();
        for (int i = 0; i < _bonusEntryList.Count; i++)
        {
            if (_bonusEntryList[i].Data != null)
                DataLoader.Instance.BonusDataList.Add(_bonusEntryList[i].Data);
        }
    }

    [Button]
    public void UpdateBonusEntryList()
    {
        _bonusEntryList.Clear();
        _bonusEntryList = GetComponentsInChildren<UI_BonusEntry>().ToList();
    }
}
