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
            _bonusEntryList[i].SetupBonus(DataLoader.Instance.TakeRandomBonusData());
        }
        Menu.OpenMenu();
    }

    public void CloseMenu()
    {
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
