using PrimeTween;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class SaveManager : MonoBehaviour
{
    #region Singleton
    public static SaveManager Instance { get; private set; }
    public static SaveData CurrentSave => SaveManager.Instance._currentSave;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            OnAwake();
        }
    }
    #endregion

    public MapNodeData CurrentMapNode;
    [SerializeField] private SaveData _currentSave;
    
    private void OnAwake()
    {
        PrimeTweenConfig.warnTweenOnDisabledTarget = false;
        InputSystem.actions.Enable();

        // in editor on awake
        LoadOrCreateSave();

        UI_Run.Instance?.PPTextValue.SetTextValue(CurrentSave.CurrentRun.ProductivityPoints.ToString(), false);
        UI_Run.Instance?.MealTicketTextValue.SetTextValue(CurrentSave.MealTickets.ToString(), false);
        SceneManager.sceneLoaded += OnSceneLoaded;

        // in build on launching in main menu
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

    public void AddMT(int number, Vector3 worldPosition = default(Vector3))
    {
        if (number > 0 && worldPosition != default(Vector3))
        {
            UI_Run.Instance?.UIParticle_MT_Attractor.onAttracted.RemoveAllListeners();
            UI_Run.Instance?.UIParticle_MT_Attractor.onAttracted.AddListener(() => OnParticleAttracted_MT(1));
            UI_Run.Instance.UIParticle_MT_RectTransform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
            UI_Run.Instance.UIParticle_MT_RectTransform.localPosition = new Vector3(UI_Run.Instance.UIParticle_MT_RectTransform.localPosition.x, UI_Run.Instance.UIParticle_MT_RectTransform.localPosition.y, 0);
            UI_Run.Instance?.UIParticle_MT.Emit(number);
        }
        else
        {
            OnParticleAttracted_MT(number);
        }
    }

    public void AddPP(int number, Vector3 worldPosition = default(Vector3))
    {
        if (number > 0 && worldPosition != default(Vector3))
        {
            UI_Run.Instance?.UIParticle_PP_Attractor.onAttracted.RemoveAllListeners();
            UI_Run.Instance?.UIParticle_PP_Attractor.onAttracted.AddListener(() => OnParticleAttracted_PP(1));
            UI_Run.Instance.UIParticle_PP_RectTransform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
            UI_Run.Instance.UIParticle_PP_RectTransform.localPosition = new Vector3(UI_Run.Instance.UIParticle_PP_RectTransform.localPosition.x, UI_Run.Instance.UIParticle_PP_RectTransform.localPosition.y, 0);
            UI_Run.Instance?.UIParticle_PP.Emit(number);
        }
        else
        {
            OnParticleAttracted_PP(number);
        }
    }

    public void OnParticleAttracted_PP(int number)
    {
        CurrentSave.CurrentRun.ProductivityPoints += number;
        if (GameManager.Instance != null) GameManager.Instance.UIManager.CoinCount.SetTextValue(CurrentSave.CurrentRun.ProductivityPoints.ToString());
        UI_Run.Instance?.PPTextValue.SetTextValue(CurrentSave.CurrentRun.ProductivityPoints.ToString());
    }

    public void OnParticleAttracted_MT(int number)
    {
        CurrentSave.MealTickets += number;
        UI_Run.Instance?.MealTicketTextValue.SetTextValue(CurrentSave.MealTickets.ToString());
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UI_Run.Instance?.PPTextValue.SetTextValue(CurrentSave.CurrentRun.ProductivityPoints.ToString(), false);
        UI_Run.Instance?.MealTicketTextValue.SetTextValue(CurrentSave.MealTickets.ToString(), false);
    }

    #region SaveManagement
    public bool LoadOrCreateSave()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/Save.json"))
        {
            LoadSave();
            return true;
        }
        else
        {
            CreateSave();
            return false;
        }
    }

    public void CreateSave()
    {
        _currentSave = new SaveData();
        _currentSave.CurrentRun = new RunData();
    }

    // Loads a save, this save must exist
    public void LoadSave()
    {
        string jsonFile = System.IO.File.ReadAllText($"{Application.persistentDataPath}/Save.json");
        _currentSave = JsonUtility.FromJson<SaveData>(jsonFile);

        // CURRENT RUN BONUS LIST
        CurrentSave.CurrentRun.CurrentRunBonusList.Clear();
        for (int i = 0; i < CurrentSave.CurrentRun.CurrentRunBonusListName.Count; i++)
        {
            CurrentSave.CurrentRun.CurrentRunBonusList.Add(DataLoader.Instance.TakeBonusByName(CurrentSave.CurrentRun.CurrentRunBonusListName[i]));
        }
        CurrentSave.CurrentRun.CurrentRunBonusListName.Clear();

        // PERMANENT BONUS
        CurrentSave.PermanentBonusList.Clear();
        for (int i = 0; i < CurrentSave.PermanentBonusListName.Count; i++)
        {
            CurrentSave.PermanentBonusList.Add(DataLoader.Instance.TakeBonusByName(CurrentSave.PermanentBonusListName[i]));
        }
        CurrentSave.PermanentBonusListName.Clear();

        // POWER UNLOCKED
        CurrentSave.UnlockedPowerDataList.Clear();
        for (int i = 0; i < CurrentSave.UnlockedPowerDataListName.Count; i++)
        {
            CurrentSave.UnlockedPowerDataList.Add(DataLoader.Instance.PowerDataList.Find(x => x.name == CurrentSave.UnlockedPowerDataListName[i]));
        }
        CurrentSave.UnlockedPowerDataListName.Clear();

    }

    // Application.persistentDataPath is : C:/Users/Username/AppData/LocalLow/{CompanyName}/{GameName}
    // Company name : DefaultCompany
    // Game name : jam-cost
    [Button("Manual Save")]
    public void Save()
    {
        // Save yarn state
        //if (YarnStorage != null)
        //{
        //    (Dictionary<string, float> yarnFloats, Dictionary<string, string> yarnStrings, Dictionary<string, bool> yarnBools) = YarnStorage.GetAllVariables();
        //    CurrentSave.YarnFloats = new SerializableDictionary<string, float>(yarnFloats);
        //    CurrentSave.YarnStrings = new SerializableDictionary<string, string>(yarnStrings);
        //    CurrentSave.YarnBools = new SerializableDictionary<string, bool>(yarnBools);
        //}
        CurrentSave.LastSceneName = SceneManager.GetActiveScene().name;

        CurrentSave.CurrentRun.CurrentRunBonusListName.Clear();
        for (int i = 0; i < CurrentSave.CurrentRun.CurrentRunBonusList.Count; i++)
        {
            CurrentSave.CurrentRun.CurrentRunBonusListName.Add(CurrentSave.CurrentRun.CurrentRunBonusList[i].name);
        }

        string save = JsonUtility.ToJson(CurrentSave, true);

        System.IO.File.WriteAllText(Application.persistentDataPath + "/Save.json", save);
    }

    private void OnDestroy()
    {
        Debug.Log("Destroy save !!!");
        Save();
    }
    #endregion

    [System.Serializable]
    public class SaveData
    {
        public string LastSceneName = "Hub";

        public int MealTickets;
        [SerializeReference] public List<BonusData> PermanentBonusList = new();
        [HideInInspector] public List<string> PermanentBonusListName = new();
        
        [SerializeReference] public List<PowerData> UnlockedPowerDataList = new();
        [HideInInspector] public List<string> UnlockedPowerDataListName = new();
        
        [SerializeReference] public List<PowerData> EquipedPowerDataList = new();
        [HideInInspector] public List<string> EquipedPowerDataListName = new();

        [SerializeReference] public int EquipedPowerMax = 1;

        // Permanent bonus stats
        public float RoundBonusTime = 0;
        public int RunStartLootPP = 0;
        public int RunStartRerolls = 0;

        public RunData CurrentRun = new RunData();

        public SaveData() { }
    }

    [System.Serializable]
    public class RunData
    {
        public int RandomSeed;
        public int CurrentDay;
        public int ProductivityPoints;
        public int Rerolls;
        public List<BonusData> CurrentRunBonusList = new();
        [HideInInspector] public List<string> CurrentRunBonusListName = new();
        public List<int> FormerNodeList = new();

        public RunData () { }
    }
}
