using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_CrateItemNumber", menuName = "Scriptable Objects/Bonus/BD_CrateItemNumber")]
public class BD_CrateItemNumber : BonusData
{
    [SerializeField] private int _itemNumber;

    public override bool CheckBonus(ref List<Item_ScoreCalculation> itemScoreCalculationList, CombinationData combinationData = null)
    {
        if (_itemNumber < 0)
        {
            return GameManager.Instance.ItemManager.ItemList.Count < -_itemNumber;
        }
        else
        {
            return GameManager.Instance.ItemManager.ItemList.Count > _itemNumber;
        }
    }

    public override string GetDescription()
    {
        string description = base.GetDescription();
        description = description.Replace("[_itemNumber]", Mathf.Abs(_itemNumber).ToString());
        return description;
    }
}
