using BennyKok.RuntimeDebug.Actions;
using BennyKok.RuntimeDebug.Attributes;
using BennyKok.RuntimeDebug.Systems;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugManager : MonoBehaviour
{

    #region Singleton
    public static DebugManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupDebugMenu();
        }
    }
    #endregion

    #region Debug Actions
    [DebugAction(name= "RoundBonusTime", group ="Info")]
    public float RoundBonusTime { get => SaveManager.Instance.CurrentSave.RoundBonusTime; set => SaveManager.Instance.CurrentSave.RoundBonusTime = value; }

    [DebugAction(name = "RunStartLoopPP", group = "Info")]
    public int RunStartLoopPP { get => SaveManager.Instance.CurrentSave.RunStartLootPP; set => SaveManager.Instance.CurrentSave.RunStartLootPP = value;}

    [DebugAction(name = "MealTickets", group = "Info")]
    public int MealTickets { get => SaveManager.Instance.CurrentSave.MealTickets; set => SaveManager.Instance.CurrentSave.MealTickets = value; }
    [DebugAction(name = "RandomSeed", group = "Info")]
    public int RandomSeed { get => SaveManager.Instance.CurrentSave.CurrentRun.RandomSeed; set => SaveManager.Instance.CurrentSave.CurrentRun.RandomSeed = value; }

    #endregion

    public List<BaseDebugAction> DebugActionList;
    public BaseDebugAction[] DebugActionArray;

    //private void OnEnable()
    //{
    //    SetupDebugMenu();
    //}

    protected virtual void OnDestroy()
    {
        RuntimeDebugSystem.UnregisterActions(DebugActionList.ToArray());
        RuntimeDebugSystem.UnregisterActions(DebugActionArray);
    }

    //private void OnDisable()
    //{
    //    RuntimeDebugSystem.UnregisterActions(DebugActionList.ToArray());
    //}

    public void SetupDebugMenu()
    {
        RuntimeDebugSystem.UnregisterActions(DebugActionList.ToArray());
        DebugActionList.Clear();

        List<BonusData> bonusDataList = Resources.LoadAll<BonusData>("Bonus").ToList();
        for (int i = 0; i < bonusDataList.Count; i++)
        {
            int index = i;
            if (bonusDataList[index].Durability == BonusData.BonusDurability.Permanent)
            {
                DebugActionList.Add(DebugActionBuilder.Button().WithName(bonusDataList[index].Name).WithGroup("Bonus/Permanent").WithAction(() => GetBonus(bonusDataList[index])));
            }
            else
            {
                DebugActionList.Add(DebugActionBuilder.Button().WithName(bonusDataList[index].Name).WithGroup("Bonus/Run").WithAction(() => GetBonus(bonusDataList[index])));
            }
        }

        // GAME
        DebugActionList.Add(DebugActionBuilder.Button().WithName("Win").WithGroup("Game").WithAction(() => WinCurrentNode()).WithClosePanelAfterTrigger(true));
        DebugActionList.Add(DebugActionBuilder.Button().WithName("Loose").WithGroup("Game").WithAction(() => WinCurrentNode()).WithClosePanelAfterTrigger(true));
        
        // MAP
        DebugActionList.Add(DebugActionBuilder.Button().WithName("Generate new map").WithGroup("Map").WithAction(() => GenerateNewMap()));
        
        // GAIN
        DebugActionList.Add(DebugActionBuilder.Button().WithName("PP").WithGroup("Gain").WithAction(() => SaveManager.Instance.AddPP(100)));
        DebugActionList.Add(DebugActionBuilder.Button().WithName("Meal Tickets").WithGroup("Gain").WithAction(() => GainMealTickets()));

        RuntimeDebugSystem.RegisterActions(DebugActionList.ToArray());
        DebugActionArray = RuntimeDebugSystem.RegisterActionsAuto(this);
    }

    public void WinCurrentNode()
    {
        GameManager.Instance?.SetGameState(GameManager.Instance.WinState);
    }

    public void GenerateNewMap()
    {
        SaveManager.Instance.CurrentSave.CurrentRun.CurrentDay = 0;
        SaveManager.Instance.CurrentSave.CurrentRun.FormerNodeList.Clear();

        SceneManager.LoadScene("Map");
    }

    public void GainMealTickets()
    {
        SaveManager.Instance.CurrentSave.MealTickets += 100;
        HubManager.Instance?.UpdateUpgrades();
    }

    public void GetBonus(BonusData data)
    {
        BonusData bonus = DataLoader.Instance.TakeSpecificBonus(data);
        if (bonus != null) bonus.GetBonus();
    }
}
