using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ClickableBehavior : ItemBehavior
{
    [SerializeField] private ParticleSystem _collectParticles;
    [SerializeField] private AudioClip _collectSound;

    private void Start()
    {
        _shadowSpriteRenderer.transform.localPosition = _offset;
        _shadowSpriteRenderer.transform.localRotation = Quaternion.identity;

        _shadowSpriteRenderer.sprite = _spriteRenderer.sprite;

        _shadowSpriteRenderer.sortingLayerName = _spriteRenderer.sortingLayerName;
        _shadowSpriteRenderer.sortingOrder = _spriteRenderer.sortingOrder - 1;
    }

    private void OnMouseDown()
    {
        if (PauseManager.Instance.IsPaused) return;
        if (GameManager.Instance.CurrentGameState != GameManager.Instance.ScavengingState
        && GameManager.Instance.CurrentGameState != GameManager.Instance.PreparationState) return;
        Collect();
    }

    public void Collect()
    {
        _collider.enabled = false;
        _collectParticles.Play();
        SaveManager.Instance.AddPP(Data.BonusCurrency);
        AudioManager.Instance.PlaySFXSound(_collectSound);
        StartCoroutine(WaitParticleDeath());
    }

    private IEnumerator WaitParticleDeath()
    {
        yield return new WaitForSeconds(_collectParticles.main.duration);
        DestroyItem();
    }
}
