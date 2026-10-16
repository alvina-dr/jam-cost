using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;

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
    public GS_ScoreCalculation BagState;
    public GS_Reward RewardState;

    [Header("References")]
    public UIManager UIManager;
    public ItemManager ItemManager;

    [Header("Boss")]
    public BossLock BossLock;

    [Header("Infos")]
    [SerializeField] private int _depotSize;
    [SerializeField] private int _roundNumber;

    [Header("Current Stats")]
    public DraggableBehavior SelectedItem;
    public int CurrentScore;
    public int GoalScore;
    public int CurrentDay;
    public int CurrentRound;
    public int CurrentDiscard;
    public int FoundPP;

    [Header("Over check")]
    public OverCheck DepotOverCheck;
    public OverCheck CrateOverCheck;

    private void Start()
    {
        switch (SaveManager.Instance.CurrentMapNode)
        {
            case MND_Scavenge_Empty:
                // prepare interface for empty challenge
                break;
            case MND_Scavenge_Possession bossPossession:
                GoalScore = SaveManager.Instance.GetScavengeNode().ScoreGoal;
                UIManager.ScoreTextValue.SetTextValue($"{CurrentScore} / {GoalScore}");
                UIManager.ScoreBarValue.SetBarValue(CurrentScore, GoalScore);
                //ScavengingState.ResetTimer();
                CurrentDiscard = GetMaxDiscardNumber();
                Instantiate(bossPossession.BossPrefab, Vector3.zero, Quaternion.identity);
                break;
            case MND_Scavenge_Classic:
                GoalScore = SaveManager.Instance.GetScavengeNode().ScoreGoal;
                UIManager.ScoreTextValue.SetTextValue($"{CurrentScore} / {GoalScore}");
                UIManager.ScoreBarValue.SetBarValue(CurrentScore, GoalScore);
                //ScavengingState.ResetTimer();
                CurrentDiscard = GetMaxDiscardNumber();
                break;
        }
        CurrentDay = 0;

        SetCurrentScore(0, false);
        UpdateCrateItemCount();
        UIManager.DiscardRemaining.SetTextValue($"{CurrentDiscard}", false);

        ItemManager.ResetDumpster();
        ScavengingState.UpdateItemNumberText();


        if (!SaveManager.CurrentSave.GameFirstTime)
        {
            Tween.Delay(.2f, () =>
            {
                DialogueManager.Instance.EndDialogueEvent += StartPlaying;
                DialogueManager.Instance.DialogueRunner.StartDialogue("Onboarding_GameScene_1");
                SaveManager.CurrentSave.GameFirstTime = true;
            });
        }
        else
        {
            CurrentGameState = ScavengingIntroState;
            CurrentGameState.EnterState();
        }
    }

    public void StartPlaying()
    {
        CurrentGameState = ScavengingIntroState;
        CurrentGameState.EnterState();
    }

    private void Update()
    {
        if (CurrentGameState) CurrentGameState.UpdateState();
    }

    public void UpdateCrateItemCount()
    {
        if (UIManager.Timer.GetTextValue() != $"{ItemManager.ItemList.Count}/{GetCrateSize()}")
            UIManager.Timer.SetTextValue($"{ItemManager.ItemList.Count}/{GetCrateSize()}");
    }


    public void SetGameState(GameState newState)
    {
        CurrentGameState.ExitState();
        CurrentGameState = newState;
        CurrentGameState.EnterState();
    }

    public void SetCurrentScore(int score, bool animation = true)
    {
        CurrentScore = score;
        UIManager.ScoreTextValue.SetTextValue($"{CurrentScore} / {GoalScore}", animation);
        UIManager.ScoreBarValue.SetBarValue(CurrentScore, GoalScore, animation);
    }

    public int GetDepotSize()
    {
        int depotSizeBonus = 0;

        List<BD_HandSize> bonusDepotSizeList = SaveManager.Instance.CheckHasRunBonusList<BD_HandSize>();

        for (int i = 0; i < bonusDepotSizeList.Count; i++)
        {
            depotSizeBonus += bonusDepotSizeList[i].BonusHandSize;
        }
        return _depotSize + depotSizeBonus;
    }

    public int GetCrateSize()
    {
        return SaveManager.Instance.GetScavengeNode().SpawnItemParameters.ItemNumber + SaveManager.CurrentSave.BonusCrateSize + SaveManager.CurrentSave.CurrentRun.RunBonusCrateSize;
    }

    public int GetMaxRoundNumber()
    {
        return SaveManager.Instance.GetScavengeNode().RoundNumber + SaveManager.CurrentSave.CurrentRun.RunBonusRound;
    }

    public int GetMaxDiscardNumber()
    {
        return SaveManager.Instance.GetScavengeNode().DiscardNumber + SaveManager.CurrentSave.CurrentRun.RunBonusDiscard;
    }

    public void RerollCrate()
    {
        if (CurrentGameState != ScavengingState) return;
        if (ScavengingState.CurrentSubState == GS_Scavenging.Scavenging_SubState.RerollCrateAnim) return;
        if (CurrentDiscard <= 0) return;

        CurrentDiscard--;
        UIManager.DiscardRemaining.SetTextValue($"{CurrentDiscard}", false);
        SaveManager.CurrentSave.NumberDiscardUsed++;
        SaveManager.Instance.GetScavengeNode().SpawnItems(ScavengingState.ItemTrashedNumber);
        ScavengingState.ItemTrashedNumber = 0;
    }

    public void ApproveDepot()
    {
        if (CurrentGameState != ScavengingState) return;
        GameManager.Instance.SetGameState(GameManager.Instance.BagState);
    }

    public void PlayAgain()
    {
        Time.timeScale = 1;
    }
}
