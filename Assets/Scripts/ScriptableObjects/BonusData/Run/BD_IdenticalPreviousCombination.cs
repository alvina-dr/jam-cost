using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_IdenticalPreviousCombination", menuName = "Scriptable Objects/Bonus/BD_IdenticalPreviousCombination")]
public class BD_IdenticalPreviousCombination : BonusData
{
    public override bool CheckBonus(ref List<Item_ScoreCalculation> itemScoreCalculationList, CombinationData combinationData = null)
    {
        return (GameManager.Instance.PreviousCombinationPlayed != null && GameManager.Instance.PreviousCombinationPlayed == ScoreCalculationManager.Instance.CurrentCombinationData);
    }
}
