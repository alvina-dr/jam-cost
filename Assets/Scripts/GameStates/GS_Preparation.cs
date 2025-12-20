using Sirenix.OdinInspector;
using UnityEngine;

public class GS_Preparation : GameState
{
    [ReadOnly] public float Timer;
    [SerializeField] private float _preparationTime;

    public override void EnterState()
    {
        base.EnterState();
        ResetTimer();
        AudioManager.Instance.StartClockSound();
        GameManager.Instance.UIManager.TicketMenu.UpdateItemNumberText();
        GameManager.Instance.UIManager.RoundRemaining.SetTextValue(GameManager.Instance.CurrentHand + "/" + SaveManager.Instance.GetScavengeNode().RoundNumber);
        GameManager.Instance.UIManager.TicketMenu.DisableMenu();
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
        GameManager.Instance.UIManager.TicketMenu.EnableMenu();
    }

    public void ResetTimer()
    {
        Timer = _preparationTime;
    }
}
