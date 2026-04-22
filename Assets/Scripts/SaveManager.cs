using EasyTransition;
using PrimeTween;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    [SerializeField] private TransitionSettings _transitionSettings;

    public MapNodeData CurrentMapNode;
    [SerializeField] private SaveData _currentSave;
    public PowerData FirstPower;
    [SerializeField] private MapNodeData _firstNode;

    [SerializeReference] public List<BonusData> PermanentBonusList = new();
    [SerializeReference] public List<PowerData> UnlockedPowerDataList = new();
    [SerializeReference] public List<PowerData> EquipedPowerDataList = new();

    [SerializeReference] public List<BonusData> CurrentRunBonusList = new();

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
        //CurrentSave.RunDataHistory.Add(CurrentSave.CurrentRun);
        CurrentSave.CurrentRun = new RunData();
        SaveManager.Instance.CurrentRunBonusList.Clear();
        CurrentMapNode = _firstNode;

        // Load permanent bonus advantages
        AddPP(CurrentSave.RunStartLootPP);
        CurrentSave.CurrentRun.Rerolls += CurrentSave.RunStartRerolls;

        if (UI_Run.Instance != null)
        {
            UI_Run.Instance.PPTextValue.SetTextValue(CurrentSave.CurrentRun.ProductivityPoints.ToString());
            UI_Run.Instance.MealTicketTextValue.SetTextValue(CurrentSave.MealTickets.ToString());
        }
    }

    public MND_Scavenge_Classic GetScavengeNode()
    {
        return (MND_Scavenge_Classic) CurrentMapNode;
    }

    public void NextNode()
    {
        CurrentSave.CurrentRun.CurrentNode++;
        //AddPP(CurrentSave.EveryNodeLootPP);
        TransitionManager.Instance().TransitionChangeScene("Map", _transitionSettings, 0);
    }

    public T CheckHasRunBonus<T>() where T : BonusData
    {
        return CurrentRunBonusList.Find(x => x is T) as T;
    }

    public List<T> CheckHasRunBonusList<T>() where T : BonusData
    {
        List<T> returnList = new();
        List<BonusData> listBonusData = new();
        listBonusData = CurrentRunBonusList.FindAll(x => x is T);

        for (int i = 0; i < listBonusData.Count; i++)
        {
            if (listBonusData[i] is T castedValue)
            {
                returnList.Add(castedValue);
            }
        }

        return returnList;
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
        if (UI_Run.Instance != null) UI_Run.Instance.PPTextValue.SetTextValue(CurrentSave.CurrentRun.ProductivityPoints.ToString());
    }

    public void OnParticleAttracted_MT(int number)
    {
        CurrentSave.MealTickets += number;
        if (UI_Run.Instance != null) UI_Run.Instance.MealTicketTextValue.SetTextValue(CurrentSave.MealTickets.ToString());
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (UI_Run.Instance != null) UI_Run.Instance.PPTextValue.SetTextValue(CurrentSave.CurrentRun.ProductivityPoints.ToString(), false);
        if (UI_Run.Instance != null) UI_Run.Instance.MealTicketTextValue.SetTextValue(CurrentSave.MealTickets.ToString(), false);
    }

    public void Update()
    {
        CurrentSave.CurrentRun.TotalRunDuration += Time.deltaTime;
        CurrentSave.TimePlayed += Time.deltaTime;
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
        PermanentBonusList.Clear();
        EquipedPowerDataList.Clear();
        UnlockedPowerDataList.Clear();
        UnlockedPowerDataList.Add(DataLoader.Instance.GetInstantiatedVersionOfPower(FirstPower));
    }

    // Loads a save, this save must exist
    public void LoadSave()
    {
        string jsonFile = System.IO.File.ReadAllText($"{Application.persistentDataPath}/Save.json");
        _currentSave = JsonUtility.FromJson<SaveData>(jsonFile);

        // Update bonus taken for Data Loader
        for (int i = 0; i < CurrentRunBonusList.Count; i++)
        {
            DataLoader.Instance.TakeRunSpecificBonus(CurrentRunBonusList[i]);
        }

        for (int i = 0; i < CurrentSave.ModifiedQuestList.Count; i++)
        {
            QuestManager.Instance.QuestDataDictionary[CurrentSave.ModifiedQuestList[i].Name].Data = CurrentSave.ModifiedQuestList[i];
        }

        for (int i = 0; i < CurrentSave.ModifiedCombinationList.Count; i++)
        {
            DataLoader.Instance.CombinationDataDictionary[CurrentSave.ModifiedCombinationList[i].Name].Data = CurrentSave.ModifiedCombinationList[i];
        }

        UnlockedPowerDataList = LoadList(CurrentSave.UnlockedPowerDataListName, DataLoader.Instance.PowerDataList);
        EquipedPowerDataList = LoadList(CurrentSave.EquipedPowerDataListName, DataLoader.Instance.PowerDataList);
        PermanentBonusList = LoadList(CurrentSave.PermanentBonusListName, DataLoader.Instance.PermanentBonusDataList);
    }

    public void EraseSave()
    {
        System.IO.File.Delete(Application.persistentDataPath + "/Save.json");
        CreateSave();
    }

    // Application.persistentDataPath is : C:/Users/Username/AppData/LocalLow/{CompanyName}/{GameName}
    // Company name : Bummin' Around
    // Game name : Slacking Off
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

        CurrentSave.ModifiedQuestList.Clear();
        List<QuestData> questDataList = QuestManager.Instance.QuestDataDictionary.Values.ToList();
        for (int i = 0; i < questDataList.Count; i++)
        {
            CurrentSave.ModifiedQuestList.Add(questDataList[i].Data);
        }

        CurrentSave.ModifiedCombinationList.Clear();
        List<CombinationData> combinationDataList = DataLoader.Instance.CombinationDataDictionary.Values.ToList();
        for (int i = 0; i < combinationDataList.Count; i++)
        {
            CurrentSave.ModifiedCombinationList.Add(combinationDataList[i].Data);
        }

        CurrentSave.UnlockedPowerDataListName = SaveList(UnlockedPowerDataList);
        CurrentSave.EquipedPowerDataListName = SaveList(EquipedPowerDataList);
        CurrentSave.PermanentBonusListName = SaveList(PermanentBonusList);

        string save = JsonUtility.ToJson(CurrentSave, true);

        System.IO.File.WriteAllText(Application.persistentDataPath + "/Save.json", save);
    }

    public void SaveRun()
    {
        for (int i = 0; i < CurrentRunBonusList.Count; i++)
        {
            CurrentSave.CurrentRun.CurrentRunBonusListName.Add(CurrentRunBonusList[i].Name);
        }

        CurrentSave.RunDataHistory.Add(CurrentSave.CurrentRun);
    }

    public List<string> SaveList<T>(List<T> currentDataList) where T : ScriptableObject
    {
        List<string> currentDataListName = new();
        for (int i = 0; i < currentDataList.Count; i++)
        {
            currentDataListName.Add(currentDataList[i].name);
        }

        return currentDataListName;
    }

    public List<T> LoadList<T>(List<string> currentDataListName, List<T> dataLib) where T : ScriptableObject
    {
        List<T> currentDataList = new();
        for (int i = 0; i < currentDataListName.Count; i++)
        {
            currentDataList.Add(dataLib.Find(x => x.name == currentDataListName[i]));
        }
        return currentDataList;
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
        [SerializeReference] public List<string> PermanentBonusListName = new();

        [SerializeReference] public List<string> UnlockedPowerDataListName = new();
        
        [SerializeReference] public List<string> EquipedPowerDataListName = new();

        [SerializeReference] public int EquipedPowerMax = 1;

        // Save stats
        public bool SeeOnboarding = false;
        public bool HubFirstTime = false;
        public bool ShopFirstTime = false;
        public bool FreeRoundFirstTime = false;
        public bool GetFreeRoundFirstTime = false;

        public bool GameFirstTime = false;
        public bool GameFirstTimeRoundPlayed = false;
        public bool GameSecondTime = false;
        public bool GameThirdTime = false;
        public bool PowerFirstTime = false;
        
        public bool SeeNewFrigo = false;
        public int NumberRunPlayed;
        public float TimePlayed;

        // Quests stats
        public int TotalPoints;
        public int PPSpentRunShop;
        public int PPConvertedToMT;
        public int NumberFirstBossPlayed;

        // Permanent bonus stats
        public float PermanentRoundBonusTime = 0;
        public int RunStartLootPP = 0;
        public int EveryNodeLootPP = 0;
        public int RunStartRerolls = 0;

        public List<QuestData.QuestDataSave> ModifiedQuestList = new();
        public List<CombinationData.CombinationDataSave> ModifiedCombinationList = new();

        public RunData CurrentRun;

        public List<RunData> RunDataHistory = new();

        public SaveData() { }
    }

    [System.Serializable]
    public class RunData
    {
        public int RandomSeed;
        public int CurrentNode;
        public int ProductivityPoints;
        public int Rerolls;
        public float TotalRunDuration;

        public float RunRoundBonusTime = 0;
        public int RunBonusRound = 0;

        public List<string> CurrentRunBonusListName = new();
        //public List<int> FormerNodeList = new();

        public RunData () 
        {
        }
    }
}
