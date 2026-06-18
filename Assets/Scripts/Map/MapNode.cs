using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapNode : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public MapNodeData MapNodeData;
    public RewardData RewardData;

    [SerializeField] private SpriteRenderer _icon;
    [SerializeField] private Transform _iconScaler;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        NodeChoiceManager.Instance.LaunchNode(MapNodeData, RewardData);
        Tween.Scale(_iconScaler, 1, .2f, Ease.InOutBack);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tween.Scale(_iconScaler, 1.1f, .2f, Ease.InOutBack);
        TooltipManager.Instance.ShowTooltip(MapNodeData, transform.position, Vector3.up * 50);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tween.Scale(_iconScaler, 1, .2f, Ease.InOutBack);
        TooltipManager.Instance.HideTooltip();
    }


}
