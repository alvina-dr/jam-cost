using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CD_Family", menuName = "Scriptable Objects/CombinationData/CD_Family")]
public class CD_Family : CombinationData
{
    public override bool CheckCombination(ref List<Item_ScoreCalculation> bagSlotListRef)
    {
        List<Item_ScoreCalculation> bagSlotList = new List<Item_ScoreCalculation>(bagSlotListRef);
        List<FamilyStat> familyCountList = new();
        for (int i = 0; i < bagSlotList.Count; i++)
        {
            FamilyStat familyStat = familyCountList.Find(x => x.Family == bagSlotList[i].ItemInstance.Data.Save.Family);
            if (familyStat != null) 
            {
                familyStat.Number++;
            }
            else
            {
                familyCountList.Add(new FamilyStat(bagSlotList[i].ItemInstance.Data.Save.Family, 1));
            }
        }
        familyCountList.Sort((a, b) => b.Number.CompareTo(a.Number));
        if (familyCountList.Count == 0) return false;
        FamilyStat max = familyCountList[0];

        if (max.Number >= 4)
        {
            Data.NumberUsed++;
            DiscoverCombination();
            bagSlotListRef = bagSlotListRef.FindAll(x => x.ItemInstance.Data.Save.Family == max.Family);
        }
        return max.Number >= 4;
    }

    private class FamilyStat
    {
        public ItemFamily Family;
        public int Number;

        public FamilyStat(ItemFamily family, int number)
        {
            Family = family;
            Number = number;
        }
    }
}
