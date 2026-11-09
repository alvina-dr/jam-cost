using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_PerItemCrate", menuName = "Scriptable Objects/Bonus/BD_PerItemCrate")]
public class BD_PerItemCrate : BonusData
{
    public override float Points => _points * GameManager.Instance.ItemManager.ItemList.Count;

    public override bool CheckBonus(ref List<Item_ScoreCalculation> itemScoreCalculationList, CombinationData combinationData = null)
    {
        return GameManager.Instance.ItemManager.ItemList.Count > 0;
    }
}
