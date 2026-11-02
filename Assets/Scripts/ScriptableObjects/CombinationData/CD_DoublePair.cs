using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CD_DoublePair", menuName = "Scriptable Objects/CombinationData/CD_DoublePair")]
public class CD_DoublePair : CombinationData
{
    [SerializeField] private bool _identical;

    public override bool CheckCombination(List<ItemData> itemDataList, ref List<Item_ScoreCalculation> itemScoreCalculationList)
    {
        List<int> validIndexList = new();

        for (int i = 0; i < itemDataList.Count; i++)
        {
            if (!validIndexList.Contains(i))
            {
                List<ItemData> pairList = itemDataList.FindAll(x => x.Save.Identity == itemDataList[i].Save.Identity);
                if (_identical) pairList = new(itemDataList.FindAll(x => x.Save.CompleteName == itemDataList[i].Save.CompleteName));

                if (pairList.Count == 2)
                {
                    for (int j = 0; j < itemDataList.Count; j++)
                    {
                        if ((_identical && itemDataList[j] == pairList[0]) || (!_identical && itemDataList[j].Save.Identity == pairList[0].Save.Identity))
                        {
                            validIndexList.Add(j);
                        }
                    }

                }
            }
        }

        if ((float) validIndexList.Count / 2.0f == 2.0f)
        {
            if (itemScoreCalculationList.Count > 0)
            {
                itemScoreCalculationList = FilterListByIndex(validIndexList, itemScoreCalculationList);
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
