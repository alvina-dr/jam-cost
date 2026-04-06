using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    #region Singleton
    public static TooltipManager Instance { get; private set; }

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private RectTransform _tooltipTransform;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _screenBorderOffset;

    public void ShowTooltip(BonusData bonusData, Vector3 position)
    {
        ShowTooltip(bonusData.Description, position);
    }

    public void ShowTooltip(CombinationData combinationData, Vector3 position)
    {
        ShowTooltip(combinationData.Description, position);
    }

    public void ShowTooltip(TooltipData tooltipData, Vector3 position)
    {
        ShowTooltip(tooltipData.Description, position);
    }

    public void ShowTooltip(ItemData itemData, Vector3 position)
    {
        ShowTooltip($"{itemData.Name} [{itemData.Price}]", position);
    }

    public void ShowTooltip(string description, Vector3 position)
    {
        _tooltipTransform.gameObject.SetActive(true);
        _descriptionText.SetText(description);
        Vector3 newPosition = Camera.main.WorldToScreenPoint(position) + _offset;

        if (newPosition.x - _tooltipTransform.sizeDelta.x / 2 < _screenBorderOffset.x)
        {
            newPosition -= new Vector3(newPosition.x - _tooltipTransform.sizeDelta.x / 2 - _screenBorderOffset.x, 0, 0);
        }
        _tooltipTransform.position = newPosition;
    }

    public void HideTooltip()
    {
        _tooltipTransform.gameObject.SetActive(false);
    }
}
