using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_RandomItemMultiplier", menuName = "Scriptable Objects/Bonus/BD_RandomItemMultiplier")]
public class BD_RandomItemMultiplier : BonusData
{
    public override bool CheckBonus(ref List<Item_ScoreCalculation> itemScoreCalculationList, CombinationData combinationData = null)
    {
        List<Item_ScoreCalculation> chosenBagSlot = new List<Item_ScoreCalculation>(itemScoreCalculationList);

        if (chosenBagSlot.Count == 0) return false;

        itemScoreCalculationList = new();
        itemScoreCalculationList.Add(chosenBagSlot[Random.Range(0, chosenBagSlot.Count)]);

        return true;
    }
}
