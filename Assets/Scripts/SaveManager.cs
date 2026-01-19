using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    #region Singleton
    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
            OnAwake();
        }
    }
    #endregion

    public MapNodeData CurrentMapNode;
    public SaveData CurrentSave;

    private void OnAwake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        CurrentSave = new SaveData();
    }

    public void StartNewRun()
    {
        CurrentSave.CurrentRun = new RunData();

        // Load permanent bonus advantages
        AddPP(CurrentSave.RunStartLootPP);

        UI_Run.Instance?.PPTextValue.SetTextValue(CurrentSave.CurrentRun.ProductivityPoints.ToString());
        UI_Run.Instance?.MealTicketTextValue.SetTextValue(CurrentSave.MealTickets.ToString());
    }

    public MND_Scavenge_Classic GetScavengeNode()
    {
        return (MND_Scavenge_Classic) CurrentMapNode;
    }

    public void NextDay()
    {
        CurrentSave.CurrentRun.CurrentDay++;
        SceneManager.LoadScene("Map");
    }

    public void AddPP(int number)
    {
        CurrentSave.CurrentRun.ProductivityPoints += number;
        GameManager.Instance?.UIManager?.CoinCount.SetTextValue(CurrentSave.CurrentRun.ProductivityPoints.ToString());
        UI_Run.Instance?.PPTextValue.SetTextValue(CurrentSave.CurrentRun.ProductivityPoints.ToString());
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UI_Run.Instance?.PPTextValue.SetTextValue(CurrentSave.CurrentRun.ProductivityPoints.ToString(), false);
        UI_Run.Instance?.MealTicketTextValue.SetTextValue(CurrentSave.MealTickets.ToString(), false);
    }

    [System.Serializable]
    public class SaveData
    {
        public int MealTickets;
        public List<BonusData> PermanentBonusList = new();

        // Permanent bonus stats
        public float RoundBonusTime;
        public int RunStartLootPP;

        public RunData CurrentRun = new RunData();

        public SaveData() { }
    }

    [System.Serializable]
    public class RunData
    {
        public int RandomSeed;
        public int CurrentDay;
        public int ProductivityPoints;
        public List<BonusData> CurrentRunBonusList = new();
        public List<int> FormerNodeList = new();

        public RunData () { }
    }
}
