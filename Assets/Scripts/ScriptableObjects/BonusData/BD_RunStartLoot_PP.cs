using UnityEngine;

[CreateAssetMenu(fileName = "BD_RunStartLoot_PP", menuName = "Scriptable Objects/Bonus/BD_RunStartLoot_PP")]
public class BD_RunStartLoot_PP : BonusData
{
    public int BonusPP;

    public override void GetBonus()
    {
        base.GetBonus();
        SaveManager.CurrentSave.RunStartLootPP += BonusPP;
    }
}
