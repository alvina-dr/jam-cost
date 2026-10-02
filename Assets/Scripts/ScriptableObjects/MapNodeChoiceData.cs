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

    public List<RewardData> GetRandomRewardList(int count)
    {
        List<RewardData> rewardList = new();
        List<RewardData> rewardPool = new(RewardDataPool);
        for (int i = 0; i < count; i++)
        {
            RewardData reward = null;
            if (rewardPool.Count > 0)
            {
                reward = rewardPool[Random.Range(0, rewardPool.Count)];
                rewardPool.Remove(reward);
            }
            else
            {
                reward = RewardDataPool[Random.Range(0, RewardDataPool.Count)];
            }

            rewardList.Add(reward);
        }

        return rewardList;
    }
}
