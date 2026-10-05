using PrimeTween;
using UnityEngine;

public class CB_Bonus : ClickableBehavior
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
        base.Collect();
        SaveManager.Instance.CurrentRunBonusList.Add(BonusData);
        GameManager.Instance.RewardState.ClearBonus(this);
        _collider.enabled = false;
        AudioManager.Instance.PlaySFXSound(_collectSound);
        Sequence hideSprite = Sequence.Create();
        hideSprite.Chain(Tween.Scale(_spriteRenderer.transform, _maxScale, .2f));
        hideSprite.Chain(Tween.Scale(_spriteRenderer.transform, 0, .1f));
        hideSprite.ChainCallback(() => BonusHandManager.Instance.AddBonus(BonusData));
    }

    protected override void OnMouseExit()
    {
        base.OnMouseExit();
        TooltipManager.Instance.HideTooltip();
    }

    protected override void OnMouseEnter()
    {
        TooltipManager.Instance.ShowTooltip(BonusData, transform.position, _tooltipOffset);

        if (!CanClickItem()) return;

        Color color = _spriteRenderer.material.GetColor("_OutlineColor");
        color = new Color(color.r, color.g, color.b, 1);
        _spriteRenderer.material.SetColor("_OutlineColor", color);
    }
}
