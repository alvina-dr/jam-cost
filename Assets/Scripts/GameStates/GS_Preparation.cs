using Sirenix.OdinInspector;
using UnityEngine;

public class GS_Preparation : GameState
{
    [ReadOnly] public float Timer;
    public float PreparationTime = 10;

    public override void EnterState()
    {
        base.EnterState();
        ResetTimer();
        AudioManager.Instance.StartClockSound();
        GameManager.Instance.ScavengingState.UpdateItemNumberText();
        GameManager.Instance.UIManager.RoundRemaining.SetTextValue(GameManager.Instance.CurrentRound + "/" + SaveManager.Instance.GetScavengeNode().RoundNumber);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Timer -= Time.deltaTime;
        if (GameManager.Instance.UIManager.Timer.GetTextValue() != Mathf.RoundToInt(Timer).ToString())
        {
            GameManager.Instance.UIManager.Timer.SetTextValue(Mathf.RoundToInt(Timer).ToString());
        }

        if (Timer <= 0)
        {
            GameManager.Instance.SetGameState(GameManager.Instance.ScavengingIntroState);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public void ResetTimer()
    {
        Timer = PreparationTime;
    }
}
