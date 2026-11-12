using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_ContainsTrash", menuName = "Scriptable Objects/Bonus/BD_ContainsTrash")]
public class BD_ContainsItem : BonusData
{
    [SerializeField] private List<ItemData> _containsItemDataList;

    public override bool CheckBonus(ref List<Item_ScoreCalculation> itemScoreCalculationList, CombinationData combinationData = null)
    {
        List<Item_ScoreCalculation> itemDataList = new List<Item_ScoreCalculation>(itemScoreCalculationList);
        List<ItemData> requiredItemList = new(_containsItemDataList);
        for (int i = 0; i < itemDataList.Count; i++)
        {
            if (requiredItemList.Find(x => x.Save.Name == itemDataList[i].ItemInstance.Data.Save.Name))
            {
                return true;
            }
        }
        return false;
    }
}
