using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Instance.ShowTooltip(Data, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }
}
