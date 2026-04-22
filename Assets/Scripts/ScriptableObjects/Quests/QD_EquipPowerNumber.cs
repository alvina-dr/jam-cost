using UnityEngine;

[CreateAssetMenu(fileName = "QD_EquipPowerNumber", menuName = "Scriptable Objects/Quests/QD_EquipPowerNumber")]
public class QD_EquipPowerNumber : QuestData
{
    public override bool CheckQuestCompletion()
    {
        if (SaveManager.Instance.EquipedPowerDataList.Count >= Goal)
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
        return SaveManager.Instance.EquipedPowerDataList.Count;
    }
}
