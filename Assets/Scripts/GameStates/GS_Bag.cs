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
        //GameManager.Instance.UIManager.BagMenu.OpenMenu();
        ScoreCalculationManager.Instance.Show();

        GameManager.Instance.UIManager.HUD_Game.gameObject.SetActive(false);
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ExitState()
    {
        base.ExitState();
        //GameManager.Instance.UIManager.BagMenu.CloseMenu();
        GameManager.Instance.UIManager.HUD_Game.gameObject.SetActive(true);
        ScoreCalculationManager.Instance.Hide();
        SaveManager.CurrentSave.GameFirstTimeRoundPlayed = true;
    }
}
