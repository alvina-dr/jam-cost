using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_BonusNumber", menuName = "Scriptable Objects/Bonus/BD_BonusNumber")]
public class BD_BonusNumber : BonusData
{
    public override float BonusValue => base.BonusValue * SaveManager.CurrentSave.CurrentRun.CurrentRunBonusList.Count;

    public override bool CheckBonus(ref List<UI_BagSlot> bagSlotListRef, List<CombinationData> combinationDataList = null)
    {
        if (SaveManager.CurrentSave.CurrentRun.CurrentRunBonusList.Count > 0) return true;
        return false;
    }
}
