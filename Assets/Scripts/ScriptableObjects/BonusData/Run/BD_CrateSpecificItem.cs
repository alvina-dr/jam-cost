using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_CrateSpecificItem", menuName = "Scriptable Objects/Bonus/BD_CrateSpecificItem")]
public class BD_CrateSpecificItem : BonusData
{
    [SerializeField] private List<ItemData> _itemDataList = new();
    [SerializeField] private bool _checkContains;

    public override bool CheckBonus(ref List<Item_ScoreCalculation> itemScoreCalculationList, CombinationData combinationData = null)
    {
        bool contains = false;
        for (int i = 0; i < _itemDataList.Count; i++)
        {
            if (GameManager.Instance.ItemManager.ItemList.Find(x => x.Item.Data == ItemDirector.GetInstantiatedItem(_itemDataList[i])) != null)
            {
                contains = true;
                break;
            }
        }

        Debug.Log("contains = " + contains);

        return (contains == _checkContains);
    }
}
