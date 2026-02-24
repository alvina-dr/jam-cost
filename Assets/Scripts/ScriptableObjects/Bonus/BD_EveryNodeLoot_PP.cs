using UnityEngine;

[CreateAssetMenu(fileName = "BD_EveryNodeLoot_PP", menuName = "Scriptable Objects/Bonus/BD_EveryNodeLoot_PP")]
public class BD_EveryNodeLoot_PP : BonusData
{
    public int BonusPP;

    public override void GetBonus()
    {
        base.GetBonus();
        SaveManager.CurrentSave.EveryNodeLootPP += BonusPP;
    }
}
