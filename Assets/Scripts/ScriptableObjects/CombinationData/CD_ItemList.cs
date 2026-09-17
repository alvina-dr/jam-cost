using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CD_ItemList", menuName = "Scriptable Objects/CombinationData/CD_ItemList")]
public class CD_ItemList : CombinationData
{
    [SerializeField] private List<ItemData> _requiredItemList = new();

    public override bool CheckCombination(ref List<UI_BagSlot> itemDataListRef)
    {
        List<UI_BagSlot> itemDataList = new List<UI_BagSlot>(itemDataListRef);
        List<ItemData> requiredItemList = ItemDirector.Instance.GetInstantiatedItemList(_requiredItemList);
        for (int i = requiredItemList.Count - 1; i >= 0; i--)
        {
            Debug.Log("try to find : " + requiredItemList[i].Save.Name);
            UI_BagSlot bagSlot = itemDataList.Find(x => x.CurrentBagItem.ItemInstance.Data.Save.Name == requiredItemList[i].Save.Name);
            
            if (bagSlot != null)
            {
                ItemData item = bagSlot.CurrentBagItem.ItemInstance.Data;
                Debug.Log("found item from combination : " + item.Save.Name);

                if (item != null)
                {
                    requiredItemList.Remove(item);
                    itemDataList.Remove(bagSlot);
                }
            }
        }

        Debug.Log("item missing number : " + requiredItemList.Count);

        if (requiredItemList.Count == 0)
        {
            Debug.Log("complete combination " + Data.Name);
            DiscoverCombination();
            Data.NumberUsed++;
            return true;
        }
        else
        {
            Debug.Log("couldn't complete combination + " + Data.Name);
            return false;
        }
    }
}
