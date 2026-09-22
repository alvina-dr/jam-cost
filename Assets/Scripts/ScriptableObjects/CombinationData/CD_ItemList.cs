using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CD_ItemList", menuName = "Scriptable Objects/CombinationData/CD_ItemList")]
public class CD_ItemList : CombinationData
{
    [SerializeField] private List<ItemData> _requiredItemList = new();

    public override bool CheckCombination(ref List<Item_ScoreCalculation> itemDataListRef)
    {
        List<Item_ScoreCalculation> itemDataList = new List<Item_ScoreCalculation>(itemDataListRef);
        List<ItemData> requiredItemList = ItemDirector.Instance.GetInstantiatedItemList(_requiredItemList);
        for (int i = requiredItemList.Count - 1; i >= 0; i--)
        {
            Item_ScoreCalculation item = itemDataList.Find(x => x.ItemInstance.Data.Save.Name == requiredItemList[i].Save.Name);
            
            if (item != null)
            {
                ItemData itemData = item.ItemInstance.Data;

                if (itemData != null)
                {
                    requiredItemList.Remove(itemData);
                    itemDataList.Remove(item);
                }
            }
        }

        if (requiredItemList.Count == 0)
        {
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
