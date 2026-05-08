using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapNodeChoiceData", menuName = "Scriptable Objects/MapNodeChoiceData")]
public class MapNodeChoiceData : ScriptableObject
{
    public List<MapNodeData> MapNodeDataPool;
    public List<RewardData> RewardDataPool;

    public RewardData ChooseRandomReward()
    {
        if (RewardDataPool.Count == 0) return null;
        return RewardDataPool[Random.Range(0, RewardDataPool.Count)];
    }
}
