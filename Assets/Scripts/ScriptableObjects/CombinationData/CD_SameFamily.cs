using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CD_Family", menuName = "Scriptable Objects/CombinationData/CD_Family")]
public class CD_Family : CombinationData
{
    public override bool CheckCombination(List<ItemData> itemDataList, ref List<Item_ScoreCalculation> itemScoreCalculationList)
    {
        List<FamilyStat> familyCountList = new();

        for (int i = 0; i < itemDataList.Count; i++)
        {
            FamilyStat familyStat = familyCountList.Find(x => x.Family == itemDataList[i].Save.Family);
            if (familyStat != null) 
            {
                familyStat.Number++;
            }
            else
            {
                familyCountList.Add(new FamilyStat(itemDataList[i].Save.Family, 1));
            }
        }
        familyCountList.Sort((a, b) => b.Number.CompareTo(a.Number));
        if (familyCountList.Count == 0) return false;
        FamilyStat max = familyCountList[0];

        if (max.Number >= 4)
        {
            if (itemScoreCalculationList.Count > 0)
            {
                itemScoreCalculationList = itemScoreCalculationList.FindAll(x => x.ItemInstance.Data.Save.Family == max.Family);
                DiscoverCombination();
                Data.NumberUsed++;
            }
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
