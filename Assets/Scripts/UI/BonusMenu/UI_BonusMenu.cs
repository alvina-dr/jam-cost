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
        for (int i = 0; i < _bonusEntryList.Count; i++)
        {
            _bonusEntryList[i].SetupBonus(DataLoader.Instance.GetRandomBonusData());
        }
        Menu.OpenMenu();
    }

    public void CloseMenu()
    {
        Menu.CloseMenu();
    }

    public void ValidateBonusSelection()
    {
        GameManager.Instance.ResetTimer();
        Time.timeScale = 1f;
    }

    public void SelectBonus(UI_BonusEntry bonusEntry)
    {
        for (int i = 0; i < _bonusEntryList.Count; i++)
        {
            if (_bonusEntryList[i] != bonusEntry) _bonusEntryList[i].DeselectBonus();
        }
    }

    [Button]
    public void UpdateBonusEntryList()
    {
        _bonusEntryList.Clear();
        _bonusEntryList = GetComponentsInChildren<UI_BonusEntry>().ToList();
    }
}
