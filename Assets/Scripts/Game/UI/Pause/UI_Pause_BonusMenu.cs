using System.Collections.Generic;
using UnityEngine;

public class UI_Pause_BonusMenu : UI_Menu
{
    [SerializeField] private List<UI_Pause_BonusEntry> _bonusEntryList = new();

    public override void OpenMenu()
    {
        Setup();
        base.OpenMenu();
    }

    public void BonusButton()
    {
        if (IsOpen()) CloseMenu();
        else OpenMenu();
    }

    public void Setup()
    {
        for (int i = 0; i < _bonusEntryList.Count; i++)
        {
            if (i < SaveManager.CurrentSave.CurrentRun.CurrentRunBonusList.Count)
            {
                _bonusEntryList[i].Setup(SaveManager.CurrentSave.CurrentRun.CurrentRunBonusList[i]);
                _bonusEntryList[i].gameObject.SetActive(true);
            }
            else
            {
                _bonusEntryList[i].gameObject.SetActive(false);
            }
        }
    }

    public override void CloseMenu()
    {
        base.CloseMenu();
    }
}
