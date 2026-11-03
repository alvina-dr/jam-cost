using UnityEngine;

[CreateAssetMenu(fileName = "BD_GetDiscard", menuName = "Scriptable Objects/Bonus/BD_GetDiscard")]
public class BD_GetDiscard : BonusData
{
    public override void GetBonus()
    {
        base.GetBonus();
        SaveManager.CurrentSave.CurrentRun.RunBonusDiscard++;
    }
}
