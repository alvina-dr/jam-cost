using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CD_ItemList", menuName = "Scriptable Objects/CombinationData/CD_ItemList")]
public class CD_ItemList : CombinationData
{
    [SerializeField] private List<ItemData> _requiredItemList = new();

    public override bool CheckCombination(List<ItemData> itemDataList, ref List<Item_ScoreCalculation> itemScoreCalculationList)
    {
        List<Item_ScoreCalculation> itemList = new List<Item_ScoreCalculation>(itemScoreCalculationList);
        List<ItemData> requiredItemList = ItemDirector.Instance.GetInstantiatedItemList(_requiredItemList);
        for (int i = requiredItemList.Count - 1; i >= 0; i--)
        {
            Item_ScoreCalculation item = itemList.Find(x => x.ItemInstance.Data.Save.Name == requiredItemList[i].Save.Name);
            
            if (item != null)
            {
                ItemData itemData = item.ItemInstance.Data;

                if (itemData != null)
                {
                    requiredItemList.Remove(itemData);
                    itemList.Remove(item);
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
