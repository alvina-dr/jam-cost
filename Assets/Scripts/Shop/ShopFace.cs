using UnityEngine;
using UnityEngine.UIElements;

public class ShopFace : Interactable
{
    [SerializeField] private ShopParentChoice _parentChoice;
    [SerializeField] private SimpleAnimation _animation;

    public void MouseEnterFace()
    {
        _animation.SetLoop(true);
        _animation.StartAnim();
        base.OnMouseEnter();
    }

    public void MouseExitFace()
    {
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
