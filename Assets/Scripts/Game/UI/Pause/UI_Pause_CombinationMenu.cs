using System.Collections.Generic;
using UnityEngine;

public class UI_Pause_CombinationMenu : UI_Menu
{
    [SerializeField] private List<UI_Pause_CombinationEntry> _combinationEntryList = new();

    public override void OpenMenu()
    {
        Setup();
        PauseManager.Instance.CloseAllMenusExcept(this);
        base.OpenMenu();
    }

    public void CombinationButton()
    {
        if (IsOpen()) CloseMenu();
        else OpenMenu();
    }

    public void Setup()
    {
        for (int i = 0; i < _combinationEntryList.Count; i++)
        {
            if (i < DataLoader.Instance.CombinationDataList.Count)
            {
                _combinationEntryList[i].Setup(DataLoader.Instance.CombinationDataList[i]);
                _combinationEntryList[i].gameObject.SetActive(true);
            }
            else
            {
                _combinationEntryList[i].gameObject.SetActive(false);
            }
        }
    }

    public override void CloseMenu()
    {
        base.CloseMenu();
    }
}
