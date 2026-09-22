using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_RandomItemMultiplier", menuName = "Scriptable Objects/Bonus/BD_RandomItemMultiplier")]
public class BD_RandomItemMultiplier : BonusData
{
    public override bool CheckBonus(ref List<Item_ScoreCalculation> itemListRef, List<CombinationData> combinationDataList = null)
    {
        List<Item_ScoreCalculation> chosenBagSlot = new List<Item_ScoreCalculation>(itemListRef);

        if (chosenBagSlot.Count == 0) return false;

        itemListRef = new();
        itemListRef.Add(chosenBagSlot[Random.Range(0, chosenBagSlot.Count)]);

        return true;
    }
}
