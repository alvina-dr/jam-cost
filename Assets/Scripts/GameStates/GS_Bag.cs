using PrimeTween;
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
        GameManager.Instance.UIManager.HUD_Game.gameObject.SetActive(false);

        Tween.Delay(1f, () => ScoreCalculationManager.Instance.Show());

        Tween.Delay(1.6f, () =>
        {
            if (!SaveManager.CurrentSave.CountScoreFirstTime)
            {
                SaveManager.CurrentSave.CountScoreFirstTime = true;
                DialogueManager.Instance.EndDialogueEvent += GameManager.Instance.PlayAgain;
                Time.timeScale = 0;
                DialogueManager.Instance.DialogueRunner.StartDialogue("Onboarding_GameScene_CountScore1");
            }
        });
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ExitState()
    {
        base.ExitState();
        GameManager.Instance.UIManager.HUD_Game.gameObject.SetActive(true);
        ScoreCalculationManager.Instance.Hide();
        SaveManager.CurrentSave.GameFirstTimeRoundPlayed = true;
    }
}
