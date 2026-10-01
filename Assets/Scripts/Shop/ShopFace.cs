using UnityEngine;

public class ShopFace : Interactable
{
    [SerializeField] private ShopParentChoice _parentChoice;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Sprite _deadFace;
    [SerializeField] private SimpleAnimation _animation;
    private bool _bonusBought;
    public void BuyBonus()
    {
        _animation.SetLoop(false);
        _animation.StopAnim();
        _sprite.sprite = _deadFace;
        _bonusBought = true;
    }

    public void MouseEnterFace()
    {
        if (_bonusBought) return;
        _animation.SetLoop(true);
        _animation.StartAnim();
        base.OnMouseEnter();
    }

    public void MouseExitFace()
    {
        if (_bonusBought) return;
        _animation.SetLoop(false);
        _animation.StopAnim();
        base.OnMouseExit();
    }

    public override void OnMouseEnter()
    {
        MouseEnterFace();
    }

    public override void OnMouseExit()
    {
        MouseExitFace();
    }
}
