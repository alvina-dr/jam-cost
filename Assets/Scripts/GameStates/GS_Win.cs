using DG.Tweening;
using UnityEngine;

public class GS_Win : GameState
{
    [SerializeField] private AudioClip _winSound;

    public override void EnterState()
    {
        base.EnterState();
        AudioManager.Instance.PlaySFXSound(_winSound);
        GameManager.Instance.UIManager.GameWon.Open();
        // show small victory animation
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
