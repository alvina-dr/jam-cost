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

    [Header("Infos")]
    public float RoundTime;
    public RoundData RoundData;
    public int HandPerRound;

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
        ScavengingIntro = 4
    }

    public GameState CurrentGameState;

    private void Start()
    {
        CurrentDay = -1;
        NextDay();
        SetGameState(GameState.ScavengingIntro);
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
        Timer = RoundTime;
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
                break;
            case GameState.CalculatingScore:
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
        }
        else
        {
            SetGameState(GameState.ChoosingBonus);
        }
    }
}
