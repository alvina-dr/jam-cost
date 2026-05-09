using DG.Tweening;
using UnityEngine;

public class CB_MT : ClickableBehavior
{
    public override void Collect()
    {
        _collider.enabled = false;
        SaveManager.Instance.AddMT(1, transform.position);
        AudioManager.Instance.PlaySFXSound(_collectSound);
        Sequence hideSprite = DOTween.Sequence();
        hideSprite.Append(_spriteRenderer.transform.DOScale(_maxScale, .2f));
        hideSprite.Append(_spriteRenderer.transform.DOScale(0, .1f));
        hideSprite.Play();
    }
}
