using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PrimeTween;

public class UI_BonusIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BonusData Data;
    [SerializeField] private Image _bonusIcon;
    public Transform InfoSpawnPoint;
    [SerializeField] private TextMeshProUGUI _description;

    public void Setup(BonusData data)
    {
        Data = data;
        _bonusIcon.sprite = data.Icon;
    }

    public void Highlight()
    {
        Tween.ShakeLocalPosition(transform, Vector3.one * 1.5f, .3f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Instance.ShowTooltip(Data, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }
}
