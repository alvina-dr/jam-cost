using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_IdenticalFirstCombination", menuName = "Scriptable Objects/Bonus/BD_IdenticalFirstCombination")]
public class BD_IdenticalFirstCombination : BonusData
{
    public override bool CheckBonus(ref List<Item_ScoreCalculation> itemScoreCalculationList, CombinationData combinationData = null)
    {
        return ScoreCalculationManager.Instance.CurrentCombinationData == GameManager.Instance.FirstCombinationPlayed;
    }
}
