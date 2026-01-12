using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.LowLevel;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    [ReadOnly] public GameState CurrentGameState;

    [Header("States")]
    public GS_Scavenging ScavengingState;
    public GS_Win WinState;
    public GS_ChoosingBonus ChoosingBonusState;
    public GS_GameOver GameOverState;
    public GS_ScavengingIntro ScavengingIntroState;
    public GS_Bag BagState;
    public GS_Preparation PreparationState;

    [Header("References")]
    public UIManager UIManager;
    public ItemManager ItemManager;

    [Header("Infos")]
    [SerializeField] private float _roundTime;
    public RoundData RoundData;
    [SerializeField] private int _ticketSize;

    [Header("Current Stats")]
    public ItemBehavior SelectedItem;
    [ReadOnly] public float Timer;
    public int CurrentScore;
    public int CurrentDay;
    public int CurrentHand;

    private void Start()
    {
        switch (SaveManager.Instance.CurrentMapNode)
        {
            case MND_Scavenge_Empty:
                // prepare interface for empty challenge
                UIManager.DayCount.gameObject.SetActive(false);
                UIManager.TicketMenu.gameObject.SetActive(false);
                break;
            case MND_Scavenge_Classic:
                UIManager.TicketMenu.GoalScoreText.SetTextValue(SaveManager.Instance.GetScavengeNode().ScoreGoal + "$", true);
                break;
        }
        CurrentDay = 0;

        UIManager.DayCount.SetTextValue((CurrentDay + 1).ToString());
        SetCurrentScore(0);
        ItemManager.ResetDumpster();
        UIManager.TicketMenu.UpdateItemNumberText();
        UIManager.BonusList.UpdateBonusList();
        UIManager?.CoinCount.SetTextValue(SaveManager.Instance.CurrentSave.ProductivityPoints.ToString());

        CurrentGameState = PreparationState;
        CurrentGameState.EnterState();
    }

    private void Update()
    {
        if (CurrentGameState) CurrentGameState.UpdateState();
    }

    public bool GetTimerPlaying()
    {
        if (UIManager.BonusMenu.Menu.IsOpen()) return false;
        return true;
    }

    public void AddBonus(BonusData bonus)
    {
        SaveManager.Instance.CurrentSave.BonusList.Add(bonus);
        UIManager.BonusList.UpdateBonusList();
    }

    public void SetGameState(GameState newState)
    {
        CurrentGameState.ExitState();
        CurrentGameState = newState;
        CurrentGameState.EnterState();

        //case GameStateEnum.Dialog:
        //    CurrentGameStateMachine = null;
        //    if (!UIManager.DialogMenu.HasBeenPlayed()) UIManager.DialogMenu.Open();
        //    else SetGameState(UIManager.DialogMenu.CurrentDialogData.EndGameState);
        //    break;
    }

    public void SetCurrentScore(int score)
    {
        CurrentScore = score;
        UIManager.TicketMenu.TotalScoreText.SetTextValue(CurrentScore.ToString() + "$");
    }

    public void CheckScoreHighEnough()
    {
        if (CurrentScore < SaveManager.Instance.GetScavengeNode().ScoreGoal) SetGameState(GameOverState);
        else SetGameState(WinState);
    }

    public int GetTicketSize()
    {
        int handSizeBonus = 0;
        List<BonusData> bonusTicketSizeList = SaveManager.Instance.CurrentSave.BonusList.FindAll(x => x is BD_HandSize);
        for (int i = 0; i < bonusTicketSizeList.Count; i++)
        {
            BD_HandSize bonusTicketSize = (BD_HandSize) bonusTicketSizeList[i];
            if (bonusTicketSize != null) handSizeBonus += bonusTicketSize.BonusHandSize;
        }
        return _ticketSize + handSizeBonus;
    }
}
