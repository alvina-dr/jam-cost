using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_LastTurn_DoubleItem", menuName = "Scriptable Objects/Bonus/BD_LastTurn_DoubleItem")]
public class BD_LastTurn_DoubleItem : BonusData
{
    public override bool CheckBonus(ref List<UI_BagSlot> bagSlotListRef, List<CombinationData> combinationDataList = null)
    {
        return GameManager.Instance.CurrentRound == GameManager.Instance.GetMaxRoundNumber();
    }
}
