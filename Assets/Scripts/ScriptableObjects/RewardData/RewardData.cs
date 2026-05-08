using UnityEngine;

[CreateAssetMenu(fileName = "RewardData", menuName = "Scriptable Objects/RewardData")]
public class RewardData : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;

    public virtual void SpawnReward()
    {

    }
}
