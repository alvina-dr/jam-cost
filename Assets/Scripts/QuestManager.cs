using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    #region Singleton
    public static QuestManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            OnAwake();
        }
    }
    #endregion

    public Dictionary<string, QuestData> QuestDataDictionary = new();

    public void OnAwake()
    {
        List<QuestData> questDataList = Resources.LoadAll<QuestData>("Quests").ToList();
        for (int i = 0; i < questDataList.Count; i++)
        {
            QuestData questData = Instantiate(questDataList[i]);
            QuestDataDictionary.Add(questData.Data.Name, questData);
        }
    }

    public void CheckAllQuestCompletion()
    {
        foreach (QuestData questData in QuestDataDictionary.Values)
        {
            questData.CheckQuestCompletion();
        }
    }

    public void CheckQuestCompletionByType<T>() where T: QuestData
    {
        List<T> questList = GetQuestListByType<T>();

        for (int i = 0; i < questList.Count; i++)
        {
            questList[i].CheckQuestCompletion();
        }
    }

    public List<T> GetQuestListByType<T>() where T : QuestData
    {
        List<T> questDataTypeList = new();
        List<QuestData> questDataList = new();
        questDataList = QuestDataDictionary.Values.ToList();
        questDataList.FindAll(x => x is T);

        for (int i = 0; i < questDataList.Count; i++)
        {
            if (questDataList[i] is T castedValue)
            {
                questDataTypeList.Add(castedValue);
            }
        }

        return questDataTypeList;
    }

    public void DiscoverQuest(QuestData questData)
    {
        questData.DiscoverQuest();
    }
}
