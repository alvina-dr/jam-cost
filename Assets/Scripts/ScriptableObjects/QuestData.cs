using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Scriptable Objects/Quests/QuestData")]
public class QuestData : ScriptableObject
{
    public QuestDataClass Data;

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

    [System.Serializable]
    public class QuestDataClass
    {
        public string Name;
        public string Description;
        public QuestState State;
    }
}
