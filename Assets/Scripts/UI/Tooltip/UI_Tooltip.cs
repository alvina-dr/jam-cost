using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private Image _image;
    [SerializeField] private Transform _leftColumn;

    public void Setup(AbstractTooltipData tooltip)
    {
        gameObject.SetActive(true);

        if (tooltip.Title == string.Empty) _titleText.gameObject.SetActive(false);
        else _titleText.gameObject.SetActive(true);
        _titleText.SetText(tooltip.Title);

        _descriptionText.SetText(tooltip.Description);

        if (tooltip.Sprite != null) _leftColumn.gameObject.SetActive(true);
        else _leftColumn.gameObject.SetActive(false);
        _image.sprite = tooltip.Sprite;
    }
}
