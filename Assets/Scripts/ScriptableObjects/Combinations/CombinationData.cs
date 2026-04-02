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

    public string Name;
    [TextArea] public string Description;
    public CombinationEffect Effect;
    public int Bonus;

    public virtual bool CheckCombination(ref List<UI_BagSlot> itemDataList)
    {
        return false;
    }
}
