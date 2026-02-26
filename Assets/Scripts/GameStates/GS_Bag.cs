using UnityEngine;

public class GS_Bag : GameState
{
    [SerializeField] private AudioClip _endRoundSound;

    public override void EnterState()
    {
        base.EnterState();
        AudioManager.Instance.StopClockSound();
        AudioManager.Instance.PlaySFXSound(_endRoundSound);
        if (GameManager.Instance.SelectedItem != null) GameManager.Instance.SelectedItem.EndDrag();
        GameManager.Instance.UIManager.HoverPrice.HidePrice();
        GameManager.Instance.UIManager.BagMenu.OpenMenu();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ExitState()
    {
        base.ExitState();
        GameManager.Instance.UIManager.BagMenu.CloseMenu();
    }
}
