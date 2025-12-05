using UnityEngine;

public class GS_Bag : GameState
{
    public override void EnterState()
    {
        base.EnterState();
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
