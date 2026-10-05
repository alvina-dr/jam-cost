using UnityEngine;

public class GS_GameOver : GameState
{
    [SerializeField] private AudioClip _looseSound;

    public override void EnterState()
    {
        base.EnterState();
        Time.timeScale = 1f;
        AudioManager.Instance.PlaySFXSound(_looseSound);
        GameManager.Instance.UIManager.HUD_Game.gameObject.SetActive(false);
        GameOverManager.Instance.Open();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
