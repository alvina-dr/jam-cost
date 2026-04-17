using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_RandomItemMultiplier", menuName = "Scriptable Objects/Bonus/BD_RandomItemMultiplier")]
public class BD_RandomItemMultiplier : BonusData
{
    public override bool CheckBonus(ref List<UI_BagSlot> bagSlotListRef, List<CombinationData> combinationDataList = null)
    {
        List<UI_BagSlot> chosenBagSlot = new List<UI_BagSlot>(bagSlotListRef);

        chosenBagSlot = chosenBagSlot.FindAll(x => x.CurrentBagItem != null);

        if (chosenBagSlot.Count == 0) return false;

        bagSlotListRef = new();
        bagSlotListRef.Add(chosenBagSlot[Random.Range(0, chosenBagSlot.Count)]);

        return true;
    }
}
