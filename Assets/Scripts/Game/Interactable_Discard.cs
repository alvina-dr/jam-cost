using UnityEngine;

public class Interactable_Discard : Interactable
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SimpleAnimation _animation;
    [SerializeField] private AnimationData _clickAnim;
    [SerializeField] private AnimationData _idleAnim;

    public void OnMouseDown()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.Instance.ScavengingState) return;

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
}
