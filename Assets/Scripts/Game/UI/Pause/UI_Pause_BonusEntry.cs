using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Pause_BonusEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _bonusIcon;
    [SerializeField] private TextMeshProUGUI _bonusName;
    [SerializeField] private TextMeshProUGUI _bonusDescription;
    [SerializeField] private Transform _tooltipParent;
    private BonusData _bonusData;

    public void Setup(BonusData bonusData)
    {
        _bonusData = bonusData;
        _bonusIcon.sprite = _bonusData.Icon;
        _bonusName.text = _bonusData.Name;
        _bonusDescription.text = _bonusData.Description;
    }

    public void ShowTooltip()
    {
        _tooltipParent.gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        _tooltipParent.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }
}
