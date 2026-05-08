using UnityEngine;

[CreateAssetMenu(fileName = "QD_PPSpentRunShop", menuName = "Scriptable Objects/Quests/QD_PPSpentRunShop")]
public class QD_PPSpentRunShop : QuestData
{
    public override bool CheckQuestCompletion()
    {
        if (SaveManager.CurrentSave.PPSpentRunShop >= Goal)
        {
            SetQuestToWaitCollection();
            return true;
        }
        else
        {
            return false;
        }
    }

    public override int GetCurrentValue()
    {
        return SaveManager.CurrentSave.PPSpentRunShop;
    }
}
