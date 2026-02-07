using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

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
            DontDestroyOnLoad(this.gameObject);
            OnAwake();
        }
    }
    #endregion

    public MapNodeData CurrentMapNode;
    public SaveData CurrentSave;
    
    private void OnAwake()
    {
        UI_Run.Instance?.PPTextValue.SetTextValue(CurrentSave.CurrentRun.ProductivityPoints.ToString(), false);
        UI_Run.Instance?.MealTicketTextValue.SetTextValue(CurrentSave.MealTickets.ToString(), false);
        SceneManager.sceneLoaded += OnSceneLoaded;

        // in editor on awake
        LoadOrCreateSave();

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

    public void AddPP(int number, Vector3 worldPosition = default(Vector3))
    {
        if (number > 0 && worldPosition != default(Vector3))
        {
            UI_Run.Instance?.UIParticle_Attractor.onAttracted.RemoveAllListeners();
            UI_Run.Instance?.UIParticle_Attractor.onAttracted.AddListener(() => OnParticleAttracted(1));
            UI_Run.Instance.UIParticle_RectTransform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
            UI_Run.Instance.UIParticle_RectTransform.localPosition = new Vector3(UI_Run.Instance.UIParticle_RectTransform.position.x, UI_Run.Instance.UIParticle_RectTransform.position.y, 0);
            UI_Run.Instance?.UIParticle_PP.Emit(number);
        }
        else
        {
            OnParticleAttracted(number);
        }
    }

    public void OnParticleAttracted(int number)
    {
        CurrentSave.CurrentRun.ProductivityPoints += number;
        if (GameManager.Instance != null) GameManager.Instance.UIManager.CoinCount.SetTextValue(CurrentSave.CurrentRun.ProductivityPoints.ToString());
        UI_Run.Instance?.PPTextValue.SetTextValue(CurrentSave.CurrentRun.ProductivityPoints.ToString());
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
            CurrentSave = new SaveData();
            return false;
        }
    }

    // Loads a save, this save must exist
    public void LoadSave()
    {
        string jsonFile = System.IO.File.ReadAllText($"{Application.persistentDataPath}/Save.json");
        CurrentSave = JsonUtility.FromJson<SaveData>(jsonFile);

        CurrentSave.CurrentRun.CurrentRunBonusList.Clear();
        for (int i = 0; i < CurrentSave.CurrentRun.CurrentRunBonusListName.Count; i++)
        {
            CurrentSave.CurrentRun.CurrentRunBonusList.Add(DataLoader.Instance.TakeBonusByName(CurrentSave.CurrentRun.CurrentRunBonusListName[i]));
        }
        CurrentSave.CurrentRun.CurrentRunBonusListName.Clear();
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
        [HideInInspector] public List<string> CurrentRunBonusListName = new();
        public List<int> FormerNodeList = new();

        public RunData () { }
    }
}
