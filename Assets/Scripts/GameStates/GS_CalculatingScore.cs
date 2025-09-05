using UnityEngine;

public class GS_CalculatingScore : GameState
{
    [SerializeField] private AudioClip _endRoundSound;

    public override void EnterState()
    {
        base.EnterState();
        AudioManager.Instance.StopClockSound();
        AudioManager.Instance.PlaySFXSound(_endRoundSound);
        if (GameManager.Instance.SelectedItem != null) GameManager.Instance.SelectedItem.EndDrag();
        GameManager.Instance.UIManager.TicketMenu.CountScore();
        GameManager.Instance.UIManager.HoverPrice.HidePrice();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
