using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class GS_Scavenging : GameState
{
    [ReadOnly] public float Timer;
    [SerializeField] private float _roundTime;

    public override void EnterState()
    {
        base.EnterState();
        GameManager.Instance.CurrentHand++;
        ResetTimer();
        AudioManager.Instance.StartClockSound();
        GameManager.Instance.UIManager.TicketMenu.UpdateItemNumberText();
        GameManager.Instance.UIManager.RoundRemaining.SetTextValue(GameManager.Instance.CurrentHand + "/" + SaveManager.Instance.GetClassicScavengeNode().RoundNumber);
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
            EndOfRound();
        }
    }

    public void EndOfRound()
    {
        ResetTimer();
        GameManager.Instance.SetGameState(GameManager.Instance.CalculatingScoreState);
    }

    public void ResetTimer()
    {
        Timer = GetRoundTime();
    }

    public float GetRoundTime()
    {
        float roundTimeBonus = 0;
        List<BonusData> bonusTimerList = SaveManager.Instance.CurrentSave.BonusList.FindAll(x => x is BD_Timer);
        for (int i = 0; i < bonusTimerList.Count; i++)
        {
            BD_Timer bonusTimerData = (BD_Timer)bonusTimerList[i];
            if (bonusTimerData != null) roundTimeBonus += bonusTimerData.BonusTime;
        }
        return _roundTime + roundTimeBonus;
    }
}
