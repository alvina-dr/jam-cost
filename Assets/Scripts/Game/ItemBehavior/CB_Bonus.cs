using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CB_Bonus : ClickableBehavior, IPointerEnterHandler, IPointerExitHandler
{
    public BonusData BonusData;

    public void Setup(BonusData bonusData)
    {
        BonusData = bonusData;
        _spriteRenderer.sprite = BonusData.Icon;
        _shadowSpriteRenderer.sprite = BonusData.Icon;
    }

    public override void Collect()
    {
        GameManager.Instance.RewardState.ClearBonus(this);
        _collider.enabled = false;
        AudioManager.Instance.PlaySFXSound(_collectSound);
        Sequence hideSprite = DOTween.Sequence();
        hideSprite.Append(_spriteRenderer.transform.DOScale(_maxScale, .2f));
        hideSprite.Append(_spriteRenderer.transform.DOScale(0, .1f));
        hideSprite.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Instance.ShowTooltip(BonusData, transform.position);
    }
}
