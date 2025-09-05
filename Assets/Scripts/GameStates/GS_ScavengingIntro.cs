using UnityEngine;

public class GS_ScavengingIntro : GameState
{
    public override void EnterState()
    {
        base.EnterState();
        GameManager.Instance.UIManager.NewHand.Show();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
