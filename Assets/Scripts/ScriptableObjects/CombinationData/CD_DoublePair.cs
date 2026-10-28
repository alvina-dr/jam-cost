using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CD_DoublePair", menuName = "Scriptable Objects/CombinationData/CD_DoublePair")]
public class CD_DoublePair : CombinationData
{
    [SerializeField] private bool _identical;

    public override bool CheckCombination(ref List<Item_ScoreCalculation> itemCalculationList)
    {
        List<Item_ScoreCalculation> itemList = new(itemCalculationList);
        List<Item_ScoreCalculation> pairItemList = new();

        for (int i = 0; i < itemList.Count; i++)
        {
            if (pairItemList.Contains(itemList[i])) continue;
            List<Item_ScoreCalculation> pairList = itemList.FindAll(x => x.ItemInstance.Data.Save.Identity == itemList[i].ItemInstance.Data.Save.Identity);
            if (_identical) pairList = itemList.FindAll(x => x.ItemInstance.Data.Save.CompleteName == itemList[i].ItemInstance.Data.Save.CompleteName);

            if (pairList.Count == 2)
            {
                for (int j = 0; j < pairList. Count; j++)
                {
                    pairItemList.Add(pairList[j]);
                }
            }
        }

        if (pairItemList.Count / 2.0f == 2.0f)
        {
            itemCalculationList = pairItemList;
            DiscoverCombination();
            Data.NumberUsed++;
            return true;
        }
        else
        {
            return false;
        }
    }
}
