using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_ItemNumber", menuName = "Scriptable Objects/Bonus/BD_ItemNumber")]
public class BD_ItemNumber : BonusData
{
    [SerializeField] private int _maxItemNumber;

    public override void GetBonus()
    {
        base.GetBonus();
    }

    public override bool CheckBonus(ref List<UI_BagSlot> itemDataListRef, List<CombinationData> combinationDataList = null)
    {
        int itemNumber = 0;

        for (int i = 0; i < itemDataListRef.Count; i++)
        {
            if (itemDataListRef[i].CurrentBagItem != null) itemNumber++;
        }

        if (itemNumber <= _maxItemNumber) return true;
        else return false;
    }
}
