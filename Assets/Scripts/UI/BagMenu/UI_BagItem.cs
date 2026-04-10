using PrimeTween;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_BagItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI_BagMenu _bagMenu;
    [SerializeField] private UI_BagSlot _currentBagSlot;
    [SerializeField] private Image _image;
    [SerializeField] private ItemData _itemData;
    [SerializeField] private ParticleSystem _countItemParticle;
    [SerializeField] private UI_Animation _highlightAnimation;
    public ItemData Data => _itemData;

    public int CurrentScore = 0;
    public List<int> CombinationItemAddList = new();
    public List<int> CombinationItemMultList = new();

    private void Start()
    {
        _bagMenu = GameManager.Instance.UIManager.BagMenu;
    }

    public void SetSlot(UI_BagSlot bagSlot)
    {
        _currentBagSlot = bagSlot;
        transform.SetParent(_currentBagSlot.BagItemParent);
    }

    public void Setup(ItemData itemData)
    {
        _itemData = itemData;
        _image.sprite = itemData.Icon;
        CurrentScore = _itemData.Price;
    }

    public void CountItem()
    {
        _countItemParticle.Play();
        Sequence sequence = Sequence.Create();
        sequence.Chain(Tween.Scale(_image.transform, 1.4f, .1f));
        sequence.Chain(Tween.Scale(_image.transform, 1, .05f));
        _highlightAnimation.StartAnim();
    }

    public void CountBaseScore()
    {
        Sequence sequence = Sequence.Create();
        sequence.Chain(Tween.Scale(_image.transform, 1.4f, .1f));
        sequence.Chain(Tween.Scale(_image.transform, 1, .05f));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Instance.ShowTooltip(Data, transform.position + Vector3.up / 2);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }
}
