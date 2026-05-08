using UnityEngine;

[CreateAssetMenu(fileName = "BD_Timer", menuName = "Scriptable Objects/Bonus/BD_Timer")]
public class BD_RoundTime : BonusData
{
    public float BonusTime;

    public override void GetBonus()
    {
        base.GetBonus();
        switch (Durability)
        {
            case BonusDurability.Run:
                SaveManager.CurrentSave.CurrentRun.RunRoundBonusTime += BonusTime;
                break;
            case BonusDurability.Permanent:
                SaveManager.CurrentSave.PermanentRoundBonusTime += BonusTime;
                break;
        }
    }
}
