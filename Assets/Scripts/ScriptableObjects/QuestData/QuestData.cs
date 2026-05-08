using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Scriptable Objects/Quests/QuestData")]
public class QuestData : ScriptableObject
{
    public QuestDataSave Data;
    public int Reward;
    [TextArea] public string Description;
    public int Goal;

    public List<QuestData> QuestUnlocked = new();

    public enum QuestState
    {
        Hidden = 0,
        New = 1,
        Completing = 2,
        WaitCollection = 3,
        Collected = 4
    }

    public void DiscoverQuest()
    {
        if (Data.State != QuestState.Hidden) return;
        Data.State = QuestState.New;
    }

    public virtual bool CheckQuestCompletion()
    {
        return false;
    }

    public virtual int GetCurrentValue()
    {
        return 0;
    }

    public virtual void SetQuestToWaitCollection()
    {
        if (Data.State == QuestState.WaitCollection || Data.State == QuestState.Collected) return;

        Data.State = QuestState.WaitCollection;
    }

    public void CollectQuest()
    {
        SaveManager.Instance.AddMT(Reward);
        Data.State = QuestState.Collected;

        for (int i = 0; i < QuestUnlocked.Count; i++)
        {
            QuestManager.Instance.GetInstantiatedQuestData(QuestUnlocked[i]).DiscoverQuest();
        }
    }

    [System.Serializable]
    public class QuestDataSave
    {
        public string Name;
        public QuestState State;
    }
}
