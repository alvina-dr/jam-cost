using UnityEngine;

public class MapNode : MonoBehaviour
{
    public MapNodeData MapNodeData;
    public RewardData RewardData;

    [SerializeField] private SpriteRenderer _icon;

    public void Setup(MapNodeData mapNodeData, RewardData rewardData)
    {
        MapNodeData = mapNodeData;
        RewardData = rewardData;
        _icon.sprite = MapNodeData.NodeIcon;

        if (MapNodeData is MND_Scavenge_Classic && rewardData)
        {
            MND_Scavenge_Classic scavengeNode = (MND_Scavenge_Classic)MapNodeData;
            _icon.sprite = rewardData.Icon;
        }
    }
}
