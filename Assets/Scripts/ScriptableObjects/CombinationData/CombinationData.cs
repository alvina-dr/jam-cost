using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombinationData", menuName = "Scriptable Objects/CombinationData")]
public class CombinationData : ScriptableObject
{
    public enum CombinationEffect
    {
        ItemAddition = 0,
        ItemMultiplication = 1,
        TotalAddition = 2,
        TotalMultiplication = 3
    }

    public enum CombinationDiscovery
    {
        Undiscovered = 0,
        New = 1,
        Discovered = 2
    }

    public CombinationDataSave Data;
    [TextArea] public string Description;
    public CombinationEffect Effect;
    public int Priority;

    [HorizontalGroup("Bonus")]
    [BoxGroup("Bonus/ADD"), HideLabel] public float BaseScore;
    [BoxGroup("Bonus/MULT"), HideLabel] public float MultiplierScore;

    public virtual bool CheckCombination(List<ItemData> itemDataList, ref List<Item_ScoreCalculation> itemScoreCalculationList)
    {
        return false;
    }

    public List<Item_ScoreCalculation> FilterListByIndex(List<int> validIndexList, List<Item_ScoreCalculation> itemScoreCalculationList)
    {
        List<Item_ScoreCalculation> returnList = new();
        for (int i = 0; i < validIndexList.Count; i++)
        {
            returnList.Add(itemScoreCalculationList[validIndexList[i]]);
        }

        return returnList;
    }

    public void DiscoverCombination()
    {
        if (Data.State != CombinationDiscovery.Undiscovered) return;
        Data.State = CombinationDiscovery.New;
    }

    public void SeeDiscoveredCombination()
    {
        if (Data.State != CombinationDiscovery.New) return;
        Data.State = CombinationDiscovery.Discovered;
    }

    [System.Serializable]
    public class CombinationDataSave
    {
        public string Name;
        public CombinationDiscovery State;
        public int NumberUsed;
    }
}
