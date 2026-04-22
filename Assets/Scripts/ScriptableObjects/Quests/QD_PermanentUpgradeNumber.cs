using UnityEngine;

[CreateAssetMenu(fileName = "QD_PermanentUpgradeNumber", menuName = "Scriptable Objects/Quests/QD_PermanentUpgradeNumber")]
public class QD_PermanentUpgradeNumber : QuestData
{
    public override bool CheckQuestCompletion()
    {
        if (SaveManager.Instance.PermanentBonusList.Count >= Goal)
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
        return SaveManager.Instance.PermanentBonusList.Count;
    }
}
