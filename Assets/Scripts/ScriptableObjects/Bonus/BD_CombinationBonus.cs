using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_CombinationBonus", menuName = "Scriptable Objects/Bonus/BD_CombinationBonus")]
public class BD_CombinationBonus : BonusData
{
    [SerializeField] private CombinationData _combinationData;

    public override void GetBonus()
    {
        base.GetBonus();
    }

    public override bool CheckBonus(ref List<UI_BagSlot> itemDataListRef, List<CombinationData> combinationDataList = null)
    {
        if (combinationDataList == null) return false;

        for (int i = 0; i < combinationDataList.Count; i++)
        {
            Debug.Log("combination data : " + combinationDataList[i].Data.Name);
            Debug.Log("Compaired with : " + _combinationData.Data.Name);
        }
        if (combinationDataList.Find(x => x.Data.Name == _combinationData.Data.Name))
        {
            return true;
        }

        return false;
    }
}
