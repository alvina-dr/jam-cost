using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    #region Singleton
    public static DataLoader Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            OnAwake();
        }
    }
    #endregion

    public List<MapNodeData> MapNodeDataList;
    public List<PowerData> PowerDataList;
    public Dictionary<string, CombinationData> CombinationDataDictionary = new();
    public Dictionary<string, ItemTagData> ItemTagDataDictionary = new();

    private void OnAwake()
    {
        MapNodeDataList = Resources.LoadAll<MapNodeData>("MapNodeData").ToList();
        MapNodeDataList = MapNodeDataList.FindAll(x => x.IsAvailableInGame);

        PowerDataList = Resources.LoadAll<PowerData>("PowerData").ToList();
        for(int i = 0;  i < PowerDataList.Count;i++)
        {
            //PowerDataList[i].Reset();
            PowerDataList[i] = Instantiate(PowerDataList[i]);
        }

        List<CombinationData> combinationDataList = Resources.LoadAll<CombinationData>("CombinationData").ToList();
        for (int i = 0; i < combinationDataList.Count; i++)
        {
            CombinationData combinationData = Instantiate(combinationDataList[i]);
            CombinationDataDictionary.Add(combinationData.Data.Name, combinationData);
        }

        List<ItemTagData> itemTagDataList = Resources.LoadAll<ItemTagData>("ItemTagData").ToList();
        for (int i = 0; i < itemTagDataList.Count; i++)
        {
            ItemTagData itemTagData = Instantiate(itemTagDataList[i]);
            ItemTagDataDictionary.Add(itemTagData.Name, itemTagData);
        }
    }


    public PowerData GetInstantiatedVersionOfPower(PowerData powerData)
    {
        return PowerDataList.Find(x => x.PowerName == powerData.PowerName);
    }

    public ItemTagData GetRandomItemTagData()
    {
        return ItemTagDataDictionary.ElementAt(Random.Range(0, ItemTagDataDictionary.Count)).Value;
    }

    public string ConvertTimeToMinutes(float time)
    {
        string minutes = System.TimeSpan.FromSeconds(time).Minutes.ToString();
        string seconds = System.TimeSpan.FromSeconds(time).Seconds.ToString();
        if (seconds.Length == 1) seconds = "0" + seconds;
        string miliseconds = System.TimeSpan.FromSeconds(time).Milliseconds.ToString();
        if (miliseconds.Length > 2) miliseconds.Substring(0, 2);
        if (miliseconds.Length == 2) miliseconds += "0";
        return (minutes + ":" + seconds);
        //return (minutes + ":" + seconds + "." + miliseconds);
    }
}
