using UnityEngine;

[CreateAssetMenu(fileName = "QD_PPConverted", menuName = "Scriptable Objects/Quests/QD_PPConverted")]
public class QD_PPConverted : QuestData
{
    public override bool CheckQuestCompletion()
    {
        if (SaveManager.CurrentSave.PPConvertedToMT >= Goal)
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
