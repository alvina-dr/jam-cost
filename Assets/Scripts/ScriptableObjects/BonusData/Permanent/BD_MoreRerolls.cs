using UnityEngine;

[CreateAssetMenu(fileName = "BD_MoreRerolls", menuName = "Scriptable Objects/Bonus/BD_MoreRerolls")]
public class BD_MoreRerolls : BonusData
{
    public override void GetBonus()
    {
        base.GetBonus();
        SaveManager.CurrentSave.RunStartRerolls += 1;
    }
}
