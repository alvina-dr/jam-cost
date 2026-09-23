using PrimeTween;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item_ScoreCalculation : MonoBehaviour
{
    [SerializeField] private UI_BagSlot _currentBagSlot;
    [SerializeField] private PolygonCollider2D _polygonCollider;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private ItemInstance _itemInstance;
    [SerializeField] private ParticleSystem _countItemParticle;
    [SerializeField] private SimpleAnimation _highlightAnimation;

    [Header("Price")]
    [SerializeField] private UI_TextValue _priceText;
    [SerializeField] private GameObject _priceGO;

    public ItemInstance ItemInstance => _itemInstance;

    public int CurrentScore = 0;
    public List<int> CombinationItemAddList = new();
    public List<int> CombinationItemMultList = new();

    [Button]
    public void UpdatePolygonCollider()
    {
        _polygonCollider.CreateFromSprite(_sprite.sprite);
    }

    public void SetSlot(UI_BagSlot bagSlot)
    {
        _currentBagSlot = bagSlot;
        transform.SetParent(_currentBagSlot.BagItemParent);
    }

    public void Setup(ItemInstance itemInstance)
    {
        gameObject.SetActive(true);
        _itemInstance = itemInstance;
        if (_itemInstance.TagData) _itemInstance.TagData.SetupTag(_sprite.material);
        _sprite.sprite = _itemInstance.Data.Icon;
        CurrentScore = _itemInstance.Data.Price;
        _priceGO.gameObject.SetActive(false);
        _polygonCollider.CreateFromSprite(_sprite.sprite);
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

    public void SetPriceText(int number)
    {
        _priceText.SetTextValue($"<wave amp=1>{number}");
        _priceGO.gameObject.SetActive(true);
    }

    public void SetPriceTextNumber(int oldPrice, int newPrice)
    {
        _priceText.SetTextValueNumber(oldPrice, newPrice, .4f);
    }

    private void OnMouseEnter()
    {
        TooltipManager.Instance.ShowTooltip(ItemInstance, transform.position, Vector3.up * 50);
    }

    private void OnMouseExit()
    {
        TooltipManager.Instance.HideTooltip();
    }
}
