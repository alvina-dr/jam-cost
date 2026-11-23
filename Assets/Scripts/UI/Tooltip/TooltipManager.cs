using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    [SerializeField] private RectTransform _tooltipTransform;
    [SerializeField] private Vector3 _screenBorderOffset;

    [SerializeField] private List<UI_Tooltip> _subTooltipList = new();

    private ScriptableObject _currentData;

    public void ShowTooltip(BonusInstance bonusInstance, Vector3 position, Vector3 offset)
    {
        List<AbstractTooltipData> tooltipList = new() { new AbstractTooltipData(bonusInstance.Data.Name, $"{bonusInstance.Data.GetDescription()}<br>{bonusInstance.Data.Rarity}") };

        for (int i = 0; i < bonusInstance.StickerList.Count; i++)
        {
            tooltipList.Add(new AbstractTooltipData(bonusInstance.StickerList[i].Data.Name, bonusInstance.StickerList[i].Data.GetDescription(), sprite:bonusInstance.StickerList[i].Data.Sprite));
        }
        ShowTooltip(tooltipList, position, offset);
    }

    public void ShowTooltip(StickerData stickerData, Vector3 position, Vector3 offset)
    {
        StringBuilder strBuilder = new StringBuilder();
        strBuilder.Append($"{stickerData.GetDescription()}<br>");
        strBuilder.Append($"{stickerData.Rarity}");
        List<AbstractTooltipData> tooltipList = new() { new AbstractTooltipData(stickerData.Name, $"{strBuilder}") };
        ShowTooltip(tooltipList, position, offset);
    }

    public void ShowTooltip(CombinationData combinationData, Vector3 position, Vector3 offset)
    {
        List<AbstractTooltipData> tooltipList = new() { new AbstractTooltipData(description:combinationData.Description) };
        ShowTooltip(tooltipList, position, offset);
    }

    public void ShowTooltip(TooltipData tooltipData, Vector3 position, Vector3 offset)
    {
        List<AbstractTooltipData> tooltipList = new() { new AbstractTooltipData(description:tooltipData.Description) };
        ShowTooltip(tooltipList, position, offset);
    }

    public void ShowTooltip(ItemInstance itemInstance, Vector3 position, Vector3 offset)
    {
        string itemTagName = itemInstance.TagData ? $" [{itemInstance.TagData.Name}]" : string.Empty;
        string itemPrice = $"[{ itemInstance.Data.Price}]";
        if (itemInstance.Data.Save.Family() == ItemFamily.Clickable)
        {
            itemPrice = string.Empty;
        }
        List<AbstractTooltipData> tooltipList = new() { new AbstractTooltipData(description: $"{itemInstance.Data.Save.Name} {itemPrice}{itemTagName}") };
        ShowTooltip(tooltipList, position, offset);
    }

    public void ShowTooltip(MapNodeData mapNodeData, Vector3 position, Vector3 offset)
    {
        List<AbstractTooltipData> tooltipList = new() { new AbstractTooltipData(description:mapNodeData.NodeName) };
        ShowTooltip(tooltipList, position, offset);
    }

    public void ShowTooltip(RewardData rewardData,  Vector3 position, Vector3 offset)
    {
        List<AbstractTooltipData> tooltipList = new() { new AbstractTooltipData(description:rewardData.Description) };
        ShowTooltip(tooltipList, position, offset);
    }

    public void ShowTooltip(List<AbstractTooltipData> abstractTooltipList, Vector3 position, Vector3 offset)
    {
        for (int i = 0; i < _subTooltipList.Count; i++)
        {
            if (i < abstractTooltipList.Count) _subTooltipList[i].Setup(abstractTooltipList[i]);
            else _subTooltipList[i].gameObject.SetActive(false);
        }

        _tooltipTransform.gameObject.SetActive(true);
        _tooltipTransform.localScale = Vector3.zero;

        StartCoroutine(UpdateSize(position, offset));
    }

    IEnumerator UpdateSize(Vector3 position, Vector3 offset)
    {
        yield return new WaitForEndOfFrame();
        Vector3 newPosition = Camera.main.WorldToScreenPoint(position) + offset;

        if (newPosition.x - _tooltipTransform.sizeDelta.x / 2 < _screenBorderOffset.x)
        {
            newPosition -= new Vector3(newPosition.x - _tooltipTransform.sizeDelta.x / 2 - _screenBorderOffset.x, 0, 0);
        }

        if (newPosition.x + _tooltipTransform.sizeDelta.x / 2 > Screen.width - _screenBorderOffset.x)
        {
            newPosition -= new Vector3(_tooltipTransform.sizeDelta.x / 2 - _screenBorderOffset.x, 0, 0);
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

        _tooltipTransform.gameObject.SetActive(false);
        _tooltipTransform.gameObject.SetActive(true);

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

public class AbstractTooltipData
{
    public string Title;
    public string Description;
    //public string Price;
    public Sprite Sprite;

    public AbstractTooltipData(string title = "", string description = "", string price = "", Sprite sprite = null)
    {
        Title = title;
        Description = description;
        //Price = price;
        Sprite = sprite;
    }
}
