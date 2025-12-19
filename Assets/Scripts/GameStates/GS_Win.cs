using DG.Tweening;
using UnityEngine;

public class GS_Win : GameState
{
    [SerializeField] private AudioClip _winSound;

    public override void EnterState()
    {
        base.EnterState();
        AudioManager.Instance.PlaySFXSound(_winSound);

        // show small victory animation

        DOVirtual.DelayedCall(1, () =>
        {
            SaveManager.Instance.NextDay();
        });
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
