using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_PerItemTrashed", menuName = "Scriptable Objects/Bonus/BD_PerItemTrashed")]
public class BD_PerItemTrashed : BonusData
{
    public override float Mult => GameManager.Instance != null ? 1.0f + (_mult * (float) GameManager.Instance.ItemManager.TrashedItemList.Count) : 1;

    public override bool CheckBonus(ref List<Item_ScoreCalculation> itemScoreCalculationList, CombinationData combinationData = null)
    {
        return GameManager.Instance.ItemManager.TrashedItemList.Count > 0;
    }
}
