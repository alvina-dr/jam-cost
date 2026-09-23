using UnityEngine;

public class GS_ScavengingIntro : GameState
{
    public override void EnterState()
    {
        base.EnterState();
        Time.timeScale = 1.0f;
        GameManager.Instance.UIManager.NewHand.Show();
        GameManager.Instance.ScavengingState.ResetTimer();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
