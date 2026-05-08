using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CD_Identical", menuName = "Scriptable Objects/CombinationData/CD_Identical")]
public class CD_Identical : CombinationData
{
    public override bool CheckCombination(ref List<UI_BagSlot> bagSlotList)
    {
        int maxIdenticalNumber = 0;
        ItemData identicalItemData = null;
        List<ItemData> itemDataList = new();
        for (int i = 0; i < bagSlotList.Count; i++)
        {
            itemDataList.Add(bagSlotList[i].CurrentBagItem.ItemInstance.Data);
        }

        for (int i = 0; i < itemDataList.Count; i++)
        {
            int num = itemDataList.FindAll(x => x.Name == itemDataList[i].Name).Count;
            if (num > maxIdenticalNumber)
            {
                maxIdenticalNumber = num;
                identicalItemData = itemDataList[i];
            }
        }

        if (maxIdenticalNumber >= 4)
        {
            bagSlotList = bagSlotList.FindAll(x => x.CurrentBagItem.ItemInstance.Data.Name == identicalItemData.Name);
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
