using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_FamilyMultiplier", menuName = "Scriptable Objects/Bonus/BD_FamilyMultiplier")]
public class BD_FamilyMultiplier : BonusData
{
    public ItemFamily FamilyBonus;

    public override bool CheckBonus(ref List<Item_ScoreCalculation> itemScoreCalculationList, CombinationData combinationData = null)
    {
        List<Item_ScoreCalculation> chosenItems = new();

        for (int i = 0; i < itemScoreCalculationList.Count; i++)
        {
            if (itemScoreCalculationList[i].ItemInstance.Data.Save.Family == FamilyBonus)
            {
                chosenItems.Add(itemScoreCalculationList[i]);
            }
        }

        itemScoreCalculationList = chosenItems;

        if (itemScoreCalculationList.Count > 0) return true;
        
        return false;
    }
}
