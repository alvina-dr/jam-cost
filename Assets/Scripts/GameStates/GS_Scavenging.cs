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
        GameManager.Instance.UIManager.RoundRemaining.SetTextValue(GameManager.Instance.CurrentHand + "/" + SaveManager.Instance.GetScavengeNode().RoundNumber);
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

        switch (SaveManager.Instance.CurrentMapNode)
        {
            case MND_Scavenge_Empty scavengeEmptyNode:
                if (GameManager.Instance.ItemManager.ItemList.Count == 0)
                {
                    GameManager.Instance.SetGameState(GameManager.Instance.WinState);
                    // win this game
                }
                break;
        }


    }

    public void EndOfRound()
    {
        switch (SaveManager.Instance.CurrentMapNode)
        {
            case MND_Scavenge_Empty scavengeEmptyNode:
                if (GameManager.Instance.ItemManager.ItemList.Count > 0)
                {
                    GameManager.Instance.SetGameState(GameManager.Instance.GameOverState);
                }
                return;
        }

        // normal classic state
        ResetTimer();
        GameManager.Instance.SetGameState(GameManager.Instance.BagState);
    }

    public void ResetTimer()
    {
        Timer = GetRoundTime();
    }

    public float GetRoundTime()
    {
        switch (SaveManager.Instance.CurrentMapNode) 
        {
            case MND_Scavenge_Empty scavengeEmptyNode :
                return scavengeEmptyNode.Timer;
        }

        return _roundTime + SaveManager.Instance.CurrentSave.RoundBonusTime;
    }
}
