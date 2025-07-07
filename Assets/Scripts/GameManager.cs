using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

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

    [Header("References")]
    public UIManager UIManager;
    public ItemManager ItemManager;

    [Header("Audio")]
    [SerializeField] private AudioClip _winSound;
    [SerializeField] private AudioClip _looseSound;
    [SerializeField] private AudioClip _endRoundSound;

    [Header("Infos")]
    [SerializeField] private float _roundTime;
    public RoundData RoundData;
    public int HandPerRound;
    [SerializeField] private int _ticketSize;

    [Header("Current Stats")]
    public ItemBehavior SelectedItem;
    [ReadOnly] public float Timer;
    public int CurrentScore;
    public List<BonusData> BonusList;
    public int CurrentDay;
    public int CurrentHand;


    public enum GameState
    {
        Scavenging = 0,
        CalculatingScore = 1,
        ChoosingBonus = 2,
        GameOver = 3,
        ScavengingIntro = 4,
        Dialog = 5
    }

    public GameState CurrentGameState;

    private void Start()
    {
        CurrentDay = -1;
        NextDay();
        UIManager.TicketMenu.UpdateItemNumberText();
        SetGameState(GameState.Dialog);
    }

    private void Update()
    {
        if (CurrentGameState != GameState.Scavenging) return;

        Timer -= Time.deltaTime;
        if (UIManager.Timer.GetTextValue() != Mathf.RoundToInt(Timer).ToString())
        {
            UIManager.Timer.SetTextValue(Mathf.RoundToInt(Timer).ToString());
        }

        if (Timer <= 0)
        {
            EndOfRound();    
        }
    }

    public bool GetTimerPlaying()
    {
        if (UIManager.BonusMenu.Menu.IsOpen()) return false;
        return true;
    }

    public void EndOfRound()
    {
        ResetTimer();
        SetGameState(GameState.CalculatingScore);
    }

    public void ResetTimer()
    {
        Timer = GetRoundTime();
    }

    public void AddBonus(BonusData bonus)
    {
        BonusList.Add(bonus);
        UIManager.BonusList.UpdateBonusList();
    }

    public void SetGameState(GameState state)
    {
        CurrentGameState = state;

        switch (state)
        {
            case GameState.Scavenging:
                CurrentHand++;
                ResetTimer();
                AudioManager.Instance.StartClockSound();
                UIManager.TicketMenu.UpdateItemNumberText();
                UIManager.RoundRemaining.SetTextValue(CurrentHand + "/" + HandPerRound);
                break;
            case GameState.CalculatingScore:
                AudioManager.Instance.StopClockSound();
                AudioManager.Instance.PlaySFXSound(_endRoundSound);
                if (SelectedItem != null) SelectedItem.EndDrag();
                UIManager.TicketMenu.CountScore();
                UIManager.HoverPrice.HidePrice();
                break;
            case GameState.ChoosingBonus:
                UIManager.BonusMenu.OpenMenu();
                break;
            case GameState.GameOver:
                UIManager.GameOver.Open();
                break;
            case GameState.ScavengingIntro:
                UIManager.NewHand.Show();
                break;
            case GameState.Dialog:
                if (!UIManager.DialogMenu.HasBeenPlayed()) UIManager.DialogMenu.Open();
                else SetGameState(UIManager.DialogMenu.CurrentDialogData.EndGameState);
                break;
        }
    }

    public void NextDay()
    {
        CurrentDay++;
        UIManager.DayCount.SetTextValue((CurrentDay + 1).ToString());

        SetCurrentScore(0);
        UIManager.TicketMenu.GoalScoreText.SetTextValue(RoundData.RoundDataList[CurrentDay].ScoreGoal + "$", true);

        ItemManager.ResetDumpster();
    }

    public void SetCurrentScore(int score)
    {
        CurrentScore = score;
        UIManager.TicketMenu.TotalScoreText.SetTextValue(CurrentScore.ToString() + "$");
    }

    public void CheckScoreHighEnough()
    {
        if (CurrentScore < RoundData.RoundDataList[CurrentDay].ScoreGoal)
        {
            SetGameState(GameState.GameOver);
            AudioManager.Instance.PlaySFXSound(_looseSound);
        }
        else
        {
            SetGameState(GameState.ChoosingBonus);
            AudioManager.Instance.PlaySFXSound(_winSound);
        }
    }

    public int GetTicketSize()
    {
        int handSizeBonus = 0;
        List<BonusData> bonusTicketSizeList = BonusList.FindAll(x => x is BD_HandSize);
        for (int i = 0; i < bonusTicketSizeList.Count; i++)
        {
            BD_HandSize bonusTicketSize = (BD_HandSize) bonusTicketSizeList[i];
            if (bonusTicketSize != null) handSizeBonus += bonusTicketSize.BonusHandSize;
        }
        return _ticketSize + handSizeBonus;
    }

    public float GetRoundTime()
    {
        float roundTimeBonus = 0;
        List<BonusData> bonusTimerList = BonusList.FindAll(x => x is BD_Timer);
        for (int i = 0; i < bonusTimerList.Count; i++)
        {
            BD_Timer bonusTimerData = (BD_Timer)bonusTimerList[i];
            if (bonusTimerData != null) roundTimeBonus += bonusTimerData.BonusTime;
        }
        return _roundTime + roundTimeBonus;
    }
}
