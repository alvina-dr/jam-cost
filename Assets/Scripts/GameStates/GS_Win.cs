using DG.Tweening;
using UnityEngine;

public class GS_Win : GameState
{
    [SerializeField] private AudioClip _winSound;

    public override void EnterState()
    {
        base.EnterState();
        Time.timeScale = 1f;
        AudioManager.Instance.PlaySFXSound(_winSound);
        GameManager.Instance.UIManager.GameWon.OpenMenu();
        // show small victory animation
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ExitState()
    {
        base.ExitState();
        GameManager.Instance.UIManager.GameWon.CloseMenu();
    }
}
