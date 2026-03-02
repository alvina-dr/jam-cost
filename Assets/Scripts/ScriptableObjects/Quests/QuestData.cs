using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Scriptable Objects/Quests/QuestData")]
public class QuestData : ScriptableObject
{
    public QuestDataClass Data;
    public int Reward;
    [TextArea] public string Description;
    public int Goal;

    public enum QuestState
    {
        New = 0,
        Completing = 1,
        WaitCollection = 2,
        Collected = 3
    }

    public virtual bool CheckQuestCompletion()
    {
        return false;
    }

    public virtual void SetQuestToWaitCollection()
    {
        Data.State = QuestState.WaitCollection;
    }

    public void CollectQuest()
    {
        SaveManager.Instance.AddMT(Reward);
        Data.State = QuestState.Collected;
    }

    [System.Serializable]
    public class QuestDataClass
    {
        public string Name;
        public QuestState State;
    }
}
