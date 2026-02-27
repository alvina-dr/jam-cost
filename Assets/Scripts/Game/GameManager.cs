using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

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
    public GS_GameOver GameOverState;
    public GS_ScavengingIntro ScavengingIntroState;
    public GS_Bag BagState;
    public GS_Preparation PreparationState;

    [Header("References")]
    public UIManager UIManager;
    public ItemManager ItemManager;

    [Header("Infos")]
    [SerializeField] private int _depotSize;

    [Header("Current Stats")]
    public DraggableBehavior SelectedItem;
    public int CurrentScore;
    public int CurrentDay;
    public int CurrentRound;

    private void Start()
    {
        switch (SaveManager.Instance.CurrentMapNode)
        {
            case MND_Scavenge_Empty:
                // prepare interface for empty challenge
                break;
            case MND_Scavenge_Classic:
                UIManager.ScoreTextValue.SetTextValue(CurrentScore.ToString() + " / " + SaveManager.Instance.GetScavengeNode().ScoreGoal.ToString());
                UIManager.ScoreBarValue.SetBarValue(CurrentScore, SaveManager.Instance.GetScavengeNode().ScoreGoal);
                break;
        }
        CurrentDay = 0;

        SetCurrentScore(0);
        ItemManager.ResetDumpster();
        ScavengingState.UpdateItemNumberText();
        UIManager.BonusList.UpdateBonusList();
        UIManager?.CoinCount.SetTextValue(SaveManager.CurrentSave.CurrentRun.ProductivityPoints.ToString());

        //List<BonusData> bonusDataFamilyList = SaveManager.CurrentSave.CurrentRun.CurrentRunBonusList.FindAll(x => x is BD_FamilyMultiplier familyMultiplier && familyMultiplier.FamilyBonus == chosenItemSlotList[index].CurrentBagItem.Data.Family);

        BD_PreparationTime bonusPreparationTime = SaveManager.Instance.CheckHasRunBonus<BD_PreparationTime>();
        if (bonusPreparationTime != null)
        {
            PreparationState.PreparationTime = bonusPreparationTime.PreparationTimeDuration;
            CurrentGameState = PreparationState;
        }
        else
        {
            CurrentGameState = ScavengingIntroState;
        }
        CurrentGameState.EnterState();
    }

    private void Update()
    {
        if (CurrentGameState) CurrentGameState.UpdateState();
    }

    public void AddBonus(BonusData bonus)
    {
        SaveManager.CurrentSave.CurrentRun.CurrentRunBonusList.Add(bonus);
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
        UIManager.ScoreTextValue.SetTextValue(CurrentScore.ToString() + " / " + SaveManager.Instance.GetScavengeNode().ScoreGoal.ToString());
        UIManager.ScoreBarValue.SetBarValue(CurrentScore, SaveManager.Instance.GetScavengeNode().ScoreGoal);
    }

    public void CheckScoreHighEnough()
    {
        if (CurrentScore < SaveManager.Instance.GetScavengeNode().ScoreGoal) SetGameState(GameOverState);
        else SetGameState(WinState);
    }

    public int GetDepotSize()
    {
        int depotSizeBonus = 0;

        List<BD_HandSize> bonusDepotSizeList = SaveManager.Instance.CheckHasRunBonusList<BD_HandSize>();

        Debug.Log("hand size list count : " + bonusDepotSizeList.Count);
        for (int i = 0; i < bonusDepotSizeList.Count; i++)
        {
            Debug.Log("depot size bonus : " + bonusDepotSizeList[i].BonusHandSize.ToString());
            depotSizeBonus += bonusDepotSizeList[i].BonusHandSize;
        }
        return _depotSize + depotSizeBonus;
    }
}
