using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CD_Family", menuName = "Scriptable Objects/CombinationData/CD_Family")]
public class CD_Family : CombinationData
{
    public override bool CheckCombination(ref List<UI_BagSlot> bagSlotListRef)
    {
        List<UI_BagSlot> bagSlotList = new List<UI_BagSlot>(bagSlotListRef);
        List<FamilyStat> familyCountList = new();
        for (int i = 0; i < bagSlotList.Count; i++)
        {
            FamilyStat familyStat = familyCountList.Find(x => x.Family == bagSlotList[i].CurrentBagItem.ItemInstance.Data.Family);
            if (familyStat != null) 
            {
                familyStat.Number++;
            }
            else
            {
                familyCountList.Add(new FamilyStat(bagSlotList[i].CurrentBagItem.ItemInstance.Data.Family, 1));
            }
        }
        familyCountList.Sort((a, b) => b.Number.CompareTo(a.Number));
        if (familyCountList.Count == 0) return false;
        FamilyStat max = familyCountList[0];

        if (max.Number >= 4)
        {
            Data.NumberUsed++;
            DiscoverCombination();
            bagSlotListRef = bagSlotListRef.FindAll(x => x.CurrentBagItem.ItemInstance.Data.Family == max.Family);
        }
        return max.Number >= 4;
    }

    private class FamilyStat
    {
        public ItemData.ItemFamily Family;
        public int Number;

        public FamilyStat(ItemData.ItemFamily family, int number)
        {
            Family = family;
            Number = number;
        }
    }
}
