using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UI_QuestsMenu : UI_Menu
{
    [SerializeField] private List<UI_QuestEntry> _questEntryList = new();

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI _questName;
    [SerializeField] private TextMeshProUGUI _questDescription;

    private QuestData _currentQuestData;
    private UI_QuestEntry _currentQuestEntry;

    public override void OpenMenu()
    {
        Setup();
        _questEntryList[0].TrySetupTicket();
        base.OpenMenu();
    }

    public void Setup()
    {
        List<QuestData> questDataList = QuestManager.Instance.QuestDataDictionary.Values.ToList();
        for (int i = 0; i < _questEntryList.Count; i++)
        {
            if (i < questDataList.Count)
            {
                _questEntryList[i].gameObject.SetActive(true);
                _questEntryList[i].Setup(questDataList[i]);
            }
            else
            {
                _questEntryList[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetupTicket(QuestData questData, UI_QuestEntry questEntry)
    {
        for (int i = 0; i < _questEntryList.Count; i++)
        {
            _questEntryList[i].Unselect();
        }

        _currentQuestData = questData;
        _currentQuestEntry = questEntry;

        if (questData == null) return;

        _questName.text = questData.Data.Name;
        _questDescription.text = questData.Data.Description;
    }

    public override void CloseMenu()
    {
        base.CloseMenu();
    }
}
