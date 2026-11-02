using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CD_SameIdentity", menuName = "Scriptable Objects/CombinationData/CD_SameIdentity")]
public class CD_SameIdentity : CombinationData
{
    [SerializeField] private int _identicalTargetNumber;

    public override bool CheckCombination(List<ItemData> itemDataList, ref List<Item_ScoreCalculation> itemScoreCalculationList)
    {
        int maxIdenticalNumber = 0;
        ItemData identicalItemData = null;

        for (int i = 0; i < itemDataList.Count; i++)
        {
            int num = itemDataList.FindAll(x => x.Save.Identity == itemDataList[i].Save.Identity).Count;
            if (num > maxIdenticalNumber)
            {
                maxIdenticalNumber = num;
                identicalItemData = itemDataList[i];
            }
        }

        if (maxIdenticalNumber >= _identicalTargetNumber)
        {
            if (itemScoreCalculationList.Count > 0)
            {
                itemScoreCalculationList = itemScoreCalculationList.FindAll(x => x.ItemInstance.Data.Save.Identity == identicalItemData.Save.Identity);
                DiscoverCombination();
                Data.NumberUsed++;
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
