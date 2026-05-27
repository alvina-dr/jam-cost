using DG.Tweening;
using System.Collections;
using System.Text;
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
    [SerializeField] private Vector3 _screenBorderOffset;

    private ScriptableObject _currentData;

    public void ShowTooltip(BonusData bonusData, Vector3 position, Vector3 offset)
    {
        StringBuilder strBuilder = new StringBuilder();
        strBuilder.Append($"<b>{bonusData.Name}</b><br>");
        strBuilder.Append($"{bonusData.Description}<br>");
        strBuilder.Append($"<b>{bonusData.Rarity}</b><br>");
        ShowTooltip(strBuilder.ToString(), position, offset);
    }

    public void ShowTooltip(CombinationData combinationData, Vector3 position, Vector3 offset)
    {
        ShowTooltip(combinationData.Description, position, offset);
    }

    public void ShowTooltip(TooltipData tooltipData, Vector3 position, Vector3 offset)
    {
        ShowTooltip(tooltipData.Description, position, offset);
    }

    public void ShowTooltip(ItemInstance itemInstance, Vector3 position, Vector3 offset)
    {
        string itemTagName = itemInstance.TagData ? $" [{itemInstance.TagData.Name}]" : string.Empty;
        ShowTooltip($"{itemInstance.Data.Save.Name} [{itemInstance.Data.Price}]{itemTagName}", position, offset);
    }

    public void ShowTooltip(MapNodeData mapNodeData, Vector3 position, Vector3 offset)
    {
        ShowTooltip(mapNodeData.NodeName, position, offset);
    }

    public void ShowTooltip(string description, Vector3 position, Vector3 offset)
    {
        _descriptionText.text = description;
        _tooltipTransform.gameObject.SetActive(true);
        _tooltipTransform.localScale = Vector3.zero;
        StartCoroutine(UpdateSize());

        IEnumerator UpdateSize()
        {
            yield return new WaitForEndOfFrame();
            Vector3 newPosition = Camera.main.WorldToScreenPoint(position) + offset;

            if (newPosition.x - _tooltipTransform.sizeDelta.x / 2 < _screenBorderOffset.x)
            {
                newPosition -= new Vector3(newPosition.x - _tooltipTransform.sizeDelta.x / 2 - _screenBorderOffset.x, 0, 0);
            }

            // ABOVE TOP
            if (newPosition.y + _tooltipTransform.sizeDelta.y > Screen.height - _screenBorderOffset.y)
            {
                _tooltipTransform.pivot = new Vector2(_tooltipTransform.pivot.x, 1);
                newPosition -= new Vector3(0, offset.y * 2, 0);
            }
            else
            {
                _tooltipTransform.pivot = new Vector2(_tooltipTransform.pivot.x, 0);
            }
            _tooltipTransform.position = newPosition;
            _tooltipTransform.localScale = Vector3.one;
        }
    }


    public void HideTooltip()
    {
        _tooltipTransform.gameObject.SetActive(false);
    }

    public bool IsDataMatch(ScriptableObject scriptableObject)
    {
        if (_currentData == null) return false;
        if (scriptableObject.GetInstanceID() == _currentData.GetInstanceID()) return true;
        return false;
    }
}
