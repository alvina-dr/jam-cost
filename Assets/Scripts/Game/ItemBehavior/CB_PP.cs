using DG.Tweening;
using UnityEngine;
using System.Collections;

public class CB_PP : ClickableBehavior
{
    public override void Collect()
    {
        base.Collect();
        _collider.enabled = false;
        SaveManager.Instance.AddPP(1, transform.position);
        GameManager.Instance.FoundPP++;
        AudioManager.Instance.PlaySFXSound(_collectSound);
        Sequence hideSprite = DOTween.Sequence();
        hideSprite.Append(_spriteRenderer.transform.DOScale(_maxScale, .2f));
        hideSprite.Append(_spriteRenderer.transform.DOScale(0, .1f));
        hideSprite.Play();
    }

    private IEnumerator WaitParticleDeath()
    {
        yield return new WaitForSeconds(_collectParticles.main.duration);
        DestroyItem();
    }
}
