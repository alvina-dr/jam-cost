using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_BonusNumber", menuName = "Scriptable Objects/Bonus/BD_BonusNumber")]
public class BD_BonusNumber : BonusData
{
    public override float Mult => base.Mult * SaveManager.Instance.CurrentRunBonusList.Count;

    public override bool CheckBonus(ref List<Item_ScoreCalculation> itemScoreCalculationList, CombinationData combinationData = null)
    {
        if (SaveManager.Instance.CurrentRunBonusList.Count > 0) return true;
        return false;
    }
}
