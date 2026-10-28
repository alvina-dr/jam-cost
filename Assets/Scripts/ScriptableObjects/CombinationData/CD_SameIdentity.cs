using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CD_SameIdentity", menuName = "Scriptable Objects/CombinationData/CD_SameIdentity")]
public class CD_SameIdentity : CombinationData
{
    [SerializeField] private int _identicalTargetNumber;

    public override bool CheckCombination(ref List<Item_ScoreCalculation> bagSlotList)
    {
        int maxIdenticalNumber = 0;
        ItemData identicalItemData = null;
        List<ItemData> itemDataList = new();
        for (int i = 0; i < bagSlotList.Count; i++)
        {
            itemDataList.Add(bagSlotList[i].ItemInstance.Data);
        }

        for (int i = 0; i < itemDataList.Count; i++)
        {
            int num = itemDataList.FindAll(x => x.Save.Identity == itemDataList[i].Save.Identity).Count;
            if (num > maxIdenticalNumber)
            {
                maxIdenticalNumber = num;
                identicalItemData = itemDataList[i];
            }
        }

        if (maxIdenticalNumber >= _identicalTargetNumber)
        {
            bagSlotList = bagSlotList.FindAll(x => x.ItemInstance.Data.Save.Identity == identicalItemData.Save.Identity);
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
