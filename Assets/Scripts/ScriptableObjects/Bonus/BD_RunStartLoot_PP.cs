using UnityEngine;

public class BD_RunStartLoot_PP : BonusData
{
    public int BonusPP;

    public override void GetBonus()
    {
        base.GetBonus();
        SaveManager.Instance.CurrentSave.RunStartLootPP += BonusPP;
    }
}
