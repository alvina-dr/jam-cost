using DG.Tweening;
using System.Collections;
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

    public void ShowTooltip(ItemInstance itemInstance, Vector3 position)
    {
        string itemTagName = itemInstance.TagData ? $" [{itemInstance.TagData.Name}]" : string.Empty;
        ShowTooltip($"{itemInstance.Data.Name} [{itemInstance.Data.Price}]{itemTagName}", position);
    }

    public void ShowTooltip(MapNodeData mapNodeData, Vector3 position)
    {
        ShowTooltip(mapNodeData.NodeName, position);
    }

    public void ShowTooltip(string description, Vector3 position)
    {
        _descriptionText.text = description;
        _tooltipTransform.gameObject.SetActive(true);
        _tooltipTransform.localScale = Vector3.zero; 
        StartCoroutine(UpdateSize());

        IEnumerator UpdateSize()
        {
            yield return new WaitForEndOfFrame();
            Vector3 newPosition = Camera.main.WorldToScreenPoint(position) + _offset;

            if (newPosition.x - _tooltipTransform.sizeDelta.x / 2 < _screenBorderOffset.x)
            {
                newPosition -= new Vector3(newPosition.x - _tooltipTransform.sizeDelta.x / 2 - _screenBorderOffset.x, 0, 0);
            }
            _tooltipTransform.position = newPosition;
            _tooltipTransform.localScale = Vector3.one;
        }
    }


    public void HideTooltip()
    {
        _tooltipTransform.gameObject.SetActive(false);
    }
}
