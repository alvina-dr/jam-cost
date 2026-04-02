using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CD_ItemList", menuName = "Scriptable Objects/CombinationData/CD_ItemList")]
public class CD_ItemList : CombinationData
{
    [SerializeField] private List<ItemData> _requiredItemList = new();

    public override bool CheckCombination(ref List<UI_BagSlot> itemDataListRef)
    {
        List<UI_BagSlot> itemDataList = new List<UI_BagSlot>(itemDataListRef);
        List<ItemData> requiredItemList = new(_requiredItemList);
        for (int i = 0; i < requiredItemList.Count; i++)
        {
            UI_BagSlot bagSlot = itemDataList.Find(x => x.CurrentBagItem.Data.Name == requiredItemList[i].Name);
            if (bagSlot != null)
            {
                ItemData item = bagSlot.CurrentBagItem.Data;
                if (item != null)
                {
                    requiredItemList.Remove(item);
                    itemDataList.Remove(bagSlot);
                }
            }
        }
        return requiredItemList.Count == 0;
    }
}
