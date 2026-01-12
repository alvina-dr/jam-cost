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
    public SaveData StartingSave;

    private void OnAwake()
    {
        CurrentSave = new SaveData(StartingSave);
        // here load save
    }

    public MND_Scavenge_Classic GetScavengeNode()
    {
        return (MND_Scavenge_Classic) CurrentMapNode;
    }

    public void NextDay()
    {
        CurrentSave.CurrentDay++;
        SceneManager.LoadScene("Map");
    }

    public void AddPP(int number)
    {
        CurrentSave.ProductivityPoints += number;
        GameManager.Instance?.UIManager?.CoinCount.SetTextValue(CurrentSave.ProductivityPoints.ToString());
    }

    [System.Serializable]
    public class SaveData
    {
        public int RandomSeed;
        public int CurrentDay;
        public int MealTickets;
        public int ProductivityPoints;
        public List<int> FormerNodeList = new();
        public List<BonusData> BonusList = new();

        public SaveData()
        {
            CurrentDay = 0;
            ProductivityPoints = 0;
        }

        public SaveData(SaveData saveData)
        {
            CurrentDay = saveData.CurrentDay;
        }
    }
}
