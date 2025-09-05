using UnityEngine;

public class GS_ChoosingBonus : GameState
{
    [SerializeField] private AudioClip _winSound;

    public override void EnterState()
    {
        base.EnterState();
        AudioManager.Instance.PlaySFXSound(_winSound);
        GameManager.Instance.UIManager.BonusMenu.OpenMenu();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
