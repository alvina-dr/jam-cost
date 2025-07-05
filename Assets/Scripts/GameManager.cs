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

    public UIManager UIManager;
    public ItemManager ItemManager;

    public ItemBehavior SelectedItem;
    public float RoundTime;
    [ReadOnly] public float Timer;
    public int CurrentScore;

    public List<BonusData> BonusList;

    public int CurrentDay;
    public RoundData RoundData;

    public enum GameState
    {
        Scavenging = 0,
        CalculatingScore = 1,
        ChoosingBonus = 2,
        GameOver = 3
    }

    public GameState CurrentGameState;

    private void Start()
    {
        CurrentDay = -1;
        SetGameState(GameState.Scavenging);
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
                NextDay();
                ItemManager.ResetDumpster();
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
        }
    }

    public void NextDay()
    {
        CurrentDay++;
        UIManager.DayCount.SetTextValue((CurrentDay + 1).ToString());
    }

    public void CalculateScore(List<ItemData> itemDataList)
    {

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
