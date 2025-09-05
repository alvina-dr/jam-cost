using UnityEngine;

public class GS_GameOver : GameState
{
    [SerializeField] private AudioClip _looseSound;

    public override void EnterState()
    {
        base.EnterState();
        AudioManager.Instance.PlaySFXSound(_looseSound);
        GameManager.Instance.UIManager.GameOver.Open();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
