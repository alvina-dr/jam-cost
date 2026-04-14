using UnityEngine;

[CreateAssetMenu(fileName = "QD_TotalPoints", menuName = "Scriptable Objects/Quests/QD_TotalPoints")]
public class QD_TotalPoints : QuestData
{
    public override bool CheckQuestCompletion()
    {
        if (SaveManager.CurrentSave.TotalPoints >= Goal)
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
        return SaveManager.CurrentSave.TotalPoints;
    }
}
