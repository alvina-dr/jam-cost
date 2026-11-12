using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_NoCondition", menuName = "Scriptable Objects/Bonus/BD_NoCondition")]
public class BD_NoCondition : BonusData
{
    public override bool CheckBonus(ref List<Item_ScoreCalculation> itemScoreCalculationList, CombinationData combinationData = null)
    {
        return true;
    }
}
