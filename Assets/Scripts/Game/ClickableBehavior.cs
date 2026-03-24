using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ClickableBehavior : ItemBehavior
{
    [SerializeField] private ParticleSystem _collectParticles;
    [SerializeField] private AudioClip _collectSound;
    [SerializeField] private float _maxScale;

    private void OnMouseDown()
    {
        if (PauseManager.Instance.IsPaused) return;
        if (!CanClickItem()) return;
        Collect();
    }

    public void Collect()
    {
        _collider.enabled = false;
        _collectParticles.Play();
        SaveManager.Instance.AddPP(Data.BonusCurrency, transform.position);
        GameManager.Instance.FoundPP++;
        AudioManager.Instance.PlaySFXSound(_collectSound);
        Sequence hideSprite = DOTween.Sequence();
        hideSprite.Append(_spriteRenderer.transform.DOScale(_maxScale, .2f));
        hideSprite.Append(_spriteRenderer.transform.DOScale(0, .1f));
        hideSprite.Play();
        StartCoroutine(WaitParticleDeath());
    }

    private IEnumerator WaitParticleDeath()
    {
        yield return new WaitForSeconds(_collectParticles.main.duration);
        DestroyItem();
    }
}
