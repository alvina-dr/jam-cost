using PrimeTween;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item_ScoreCalculation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private UI_BagSlot _currentBagSlot;
    [SerializeField] private PolygonCollider2D _polygonCollider;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private ItemInstance _itemInstance;
    [SerializeField] private ParticleSystem _countItemParticle;
    [SerializeField] private UI_Animation _highlightAnimation;
    public ItemInstance ItemInstance => _itemInstance;

    public int CurrentScore = 0;
    public List<int> CombinationItemAddList = new();
    public List<int> CombinationItemMultList = new();

    public void SetSlot(UI_BagSlot bagSlot)
    {
        _currentBagSlot = bagSlot;
        transform.SetParent(_currentBagSlot.BagItemParent);
    }

    public void Setup(ItemInstance itemInstance)
    {
        _itemInstance = itemInstance;
        if (_itemInstance.TagData) _itemInstance.TagData.SetupTag(_sprite.material);
        _sprite.sprite = _itemInstance.Data.Icon;
        CurrentScore = _itemInstance.Data.Price;
    }

    public void CountItem()
    {
        _countItemParticle.Play();
        Sequence sequence = Sequence.Create();
        sequence.Chain(Tween.Scale(_sprite.transform, 1.4f, .1f));
        sequence.Chain(Tween.Scale(_sprite.transform, 1, .05f));
        _highlightAnimation.StartAnim();
    }

    public void CountBaseScore()
    {
        Sequence sequence = Sequence.Create();
        sequence.Chain(Tween.Scale(_sprite.transform, 1.4f, .1f));
        sequence.Chain(Tween.Scale(_sprite.transform, 1, .05f));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Instance.ShowTooltip(ItemInstance, transform.position, Vector3.up * 50);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }
}
