using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_FamilyMultiplier", menuName = "Scriptable Objects/Bonus/BD_FamilyMultiplier")]
public class BD_FamilyMultiplier : BonusData
{
    public ItemData.ItemFamily FamilyBonus;

    public override bool CheckBonus(ref List<UI_BagSlot> bagSlotListRef, List<CombinationData> combinationDataList = null)
    {
        List<UI_BagSlot> chosenBagSlots = new();

        for (int i = 0; i < bagSlotListRef.Count; i++)
        {
            if (bagSlotListRef[i].CurrentBagItem != null 
                && bagSlotListRef[i].CurrentBagItem.Data.Family == FamilyBonus)
            {
                chosenBagSlots.Add(bagSlotListRef[i]);
            }
        }

        bagSlotListRef = chosenBagSlots;

        if (bagSlotListRef.Count > 0) return true;
        
        return false;
    }
}
