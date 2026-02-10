using UnityEngine;

[CreateAssetMenu(fileName = "BD_Timer", menuName = "Scriptable Objects/Bonus/BD_Timer")]
public class BD_Timer : BonusData
{
    public float BonusTime;

    public override void GetBonus()
    {
        base.GetBonus();
        SaveManager.CurrentSave.RoundBonusTime += BonusTime;
    }
}
