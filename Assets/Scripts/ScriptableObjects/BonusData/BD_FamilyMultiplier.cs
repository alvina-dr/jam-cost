using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_FamilyMultiplier", menuName = "Scriptable Objects/Bonus/BD_FamilyMultiplier")]
public class BD_FamilyMultiplier : BonusData
{
    public ItemFamily FamilyBonus;

    public override bool CheckBonus(ref List<Item_ScoreCalculation> bagSlotListRef, List<CombinationData> combinationDataList = null)
    {
        List<Item_ScoreCalculation> chosenItems = new();

        for (int i = 0; i < bagSlotListRef.Count; i++)
        {
            if (bagSlotListRef[i].ItemInstance.Data.Save.Family == FamilyBonus)
            {
                chosenItems.Add(bagSlotListRef[i]);
            }
        }

        bagSlotListRef = chosenItems;

        if (bagSlotListRef.Count > 0) return true;
        
        return false;
    }
}
