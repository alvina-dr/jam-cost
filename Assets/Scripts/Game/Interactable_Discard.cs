using Unity.VisualScripting;
using UnityEngine;

public class Interactable_Discard : Interactable
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SimpleAnimation _animation;
    [SerializeField] private AnimationData _clickAnim;
    [SerializeField] private AnimationData _idleAnim;
    [SerializeField] private Transform _tooltipOrigin;

    public void OnMouseDown()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.Instance.ScavengingState) return;
        if (GameManager.Instance.CurrentDiscard <= 0) return;

        _collider.enabled = false;
        _animation.SetAnimation(_clickAnim);
        _animation.SetLoop(false);
        _animation.StartAnim(() =>
        {
            _collider.enabled = true;
            _animation.SetAnimation(_idleAnim);
            _animation.SetLoop(true);
            _animation.StartAnim();
        });
        GameManager.Instance.RerollCrate();
    }

    public override void OnMouseEnter()
    {
        base.OnMouseEnter();
        TooltipManager.Instance.ShowTooltip($"Refill (Discarded items : {GameManager.Instance.ScavengingState.ItemTrashedNumber})",
           _tooltipOrigin.position, Vector3.zero);
    }

    public override void OnMouseExit()
    {
        base.OnMouseExit();
        TooltipManager.Instance.HideTooltip();
    }
}
