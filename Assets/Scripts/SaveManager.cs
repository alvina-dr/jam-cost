using System.Collections.Generic;
using UnityEngine;

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

    public MND_ClassicScavenge GetClassicScavengeNode()
    {
        return (MND_ClassicScavenge) CurrentMapNode;
    }

    [System.Serializable]
    public class SaveData
    {
        public int RandomSeed;
        public int CurrentDay;
        public List<int> FormerNodeList = new();

        public SaveData()
        {
            CurrentDay = 0;
        }

        public SaveData(SaveData saveData)
        {
            CurrentDay = saveData.CurrentDay;
        }
    }
}
