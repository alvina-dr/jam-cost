using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CD_Identical", menuName = "Scriptable Objects/CombinationData/CD_Identical")]
public class CD_Identical : CombinationData
{
    [SerializeField] private int _identicalTargetNumber;

    public override bool CheckCombination(ref List<Item_ScoreCalculation> itemCalculationList)
    {
        int maxIdenticalNumber = 0;
        ItemData identicalItemData = null;
        List<ItemData> itemDataList = new();
        for (int i = 0; i < itemCalculationList.Count; i++)
        {
            itemDataList.Add(itemCalculationList[i].ItemInstance.Data);
        }

        for (int i = 0; i < itemDataList.Count; i++)
        {
            int num = itemDataList.FindAll(x => x.Save.CompleteName == itemDataList[i].Save.CompleteName).Count;
            if (num > maxIdenticalNumber)
            {
                maxIdenticalNumber = num;
                identicalItemData = itemDataList[i];
            }
        }

        if (maxIdenticalNumber >= _identicalTargetNumber)
        {
            itemCalculationList = itemCalculationList.FindAll(x => x.ItemInstance.Data.Save.CompleteName == identicalItemData.Save.CompleteName);
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
