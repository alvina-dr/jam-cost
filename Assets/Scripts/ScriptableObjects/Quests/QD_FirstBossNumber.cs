using UnityEngine;

[CreateAssetMenu(fileName = "QD_FirstBossNumber", menuName = "Scriptable Objects/Quests/QD_FirstBossNumber")]
public class QD_FirstBossNumber : QuestData
{
    public override bool CheckQuestCompletion()
    {
        if (SaveManager.CurrentSave.NumberFirstBossPlayed >= Goal)
        {
            SetQuestToWaitCollection();
            return true;
        }
        else
        {
            return false;
        }
    }
}
