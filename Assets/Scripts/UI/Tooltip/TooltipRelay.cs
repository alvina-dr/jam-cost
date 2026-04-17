using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipRelay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TooltipData _tooltipData;
    [SerializeField] private Vector3 _offset;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Instance.ShowTooltip(_tooltipData, transform.position + _offset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }
}
