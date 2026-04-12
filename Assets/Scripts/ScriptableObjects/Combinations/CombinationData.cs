using System.Collections.Generic;
using UnityEngine;
using static QuestData;

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
    public int Bonus;

    public virtual bool CheckCombination(ref List<UI_BagSlot> itemDataList)
    {
        return false;
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
    }
}
