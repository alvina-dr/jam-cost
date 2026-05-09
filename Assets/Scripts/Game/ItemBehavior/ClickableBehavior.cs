using DG.Tweening;
using UnityEngine;

public class ClickableBehavior : ItemBehavior
{
    [SerializeField] protected ParticleSystem _collectParticles;
    [SerializeField] protected AudioClip _collectSound;
    [SerializeField] protected float _maxScale;

    private void OnMouseDown()
    {
        if (PauseManager.Instance.IsPaused) return;
        if (!CanClickItem()) return;
        Collect();
    }

    public virtual void Collect()
    {

    }
}
