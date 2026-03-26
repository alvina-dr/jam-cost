using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CD_Family", menuName = "Scriptable Objects/CombinationData/CD_Family")]
public class CD_Family : CombinationData
{
    public ItemData.ItemFamily CombinationFamily;

    public override bool CheckCombination(List<UI_BagSlot> itemDataList)
    {
        int numberFamily = 0;
        for (int i = 0; i < itemDataList.Count; i++)
        {
            if (itemDataList[i].CurrentBagItem.Data.Family == CombinationFamily) numberFamily++;
        }
        return numberFamily >= 4;
    }
}
