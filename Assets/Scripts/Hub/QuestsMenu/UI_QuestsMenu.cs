using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class UI_QuestsMenu : UI_Menu
{
    [SerializeField] private List<UI_QuestEntry> _questEntryList = new();

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI _questName;
    [SerializeField] private TextMeshProUGUI _questDescription;
    [SerializeField] private TextMeshProUGUI _questButtonText;

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

        List<QuestData> orderedQuestDataList = new();

        orderedQuestDataList.AddRange(questDataList.FindAll(x => x.Data.State == QuestData.QuestState.WaitCollection));
        orderedQuestDataList.AddRange(questDataList.FindAll(x => x.Data.State == QuestData.QuestState.New));
        orderedQuestDataList.AddRange(questDataList.FindAll(x => x.Data.State == QuestData.QuestState.Completing));
        orderedQuestDataList.AddRange(questDataList.FindAll(x => x.Data.State == QuestData.QuestState.Collected));


        for (int i = 0; i < _questEntryList.Count; i++)
        {
            if (i < orderedQuestDataList.Count)
            {
                _questEntryList[i].gameObject.SetActive(true);
                _questEntryList[i].Setup(orderedQuestDataList[i]);
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
        _questDescription.text = questData.Description;

        switch (_currentQuestData.Data.State)
        {
            case QuestData.QuestState.New:
            case QuestData.QuestState.Completing:
                _questButtonText.text = $"{questData.Reward} <sprite name=MT>";
                break;
            case QuestData.QuestState.WaitCollection:
                _questButtonText.text = $"Collect ({questData.Reward} <sprite name=MT>)";
                break;
            case QuestData.QuestState.Collected:
                _questButtonText.text = "Complete";
                break;
        }
    }

    public override void CloseMenu()
    {
        base.CloseMenu();
    }

    public void TicketButton()
    {
        if (_currentQuestData.Data.State ==  QuestData.QuestState.WaitCollection)
        {
            _currentQuestData.CollectQuest();
            HubManager.Instance.UpdateAllUnlocks();
            Setup();
            _currentQuestEntry.TrySetupTicket();
        }
    }
}
