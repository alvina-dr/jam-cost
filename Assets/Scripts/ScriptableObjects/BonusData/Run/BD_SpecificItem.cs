using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_SpecificItem", menuName = "Scriptable Objects/Bonus/BD_SpecificItem")]
public class BD_SpecificItem : BonusData
{
    [SerializeField] private List<ItemData> _itemDataList = new();
    public override bool CheckBonus(ref List<Item_ScoreCalculation> itemScoreCalculationList, CombinationData combinationData = null)
    {
        _itemDataList = ItemDirector.Instance.GetInstantiatedItemList(_itemDataList);

        List<Item_ScoreCalculation> returnList = new();
        for (int i = 0; i < itemScoreCalculationList.Count; i++)
        {
            if (_itemDataList.Contains(itemScoreCalculationList[i].ItemInstance.Data))
            {
                returnList.Add(itemScoreCalculationList[i]);
            }
        }
        itemScoreCalculationList = returnList;
        return returnList.Count > 0;
    }
}
