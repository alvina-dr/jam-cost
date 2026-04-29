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

    public List<ItemData> ItemDataList;
    public List<BonusData> RunBonusDataList;
    public List<BonusData> PermanentBonusDataList;
    public List<MapNodeData> MapNodeDataList;
    public List<PowerData> PowerDataList;
    public Dictionary<string, CombinationData> CombinationDataDictionary = new();
    public Dictionary<string, ItemTagData> ItemTagDataDictionary = new();

    private void OnAwake()
    {
        ItemDataList = Resources.LoadAll<ItemData>("Items").ToList();
        for (int i = 0; i < ItemDataList.Count; i++)
        {
            ItemDataList[i] = Instantiate(ItemDataList[i]);
        }

        RunBonusDataList = Resources.LoadAll<BonusData>("Bonus/Run").ToList();
        RunBonusDataList = RunBonusDataList.FindAll(x => x.IsAvailableInGame);

        PermanentBonusDataList = Resources.LoadAll<BonusData>("Bonus/Permanent").ToList();
        PermanentBonusDataList = PermanentBonusDataList.FindAll(x => x.IsAvailableInGame);

        MapNodeDataList = Resources.LoadAll<MapNodeData>("MapNodes").ToList();
        MapNodeDataList = MapNodeDataList.FindAll(x => x.IsAvailableInGame);

        PowerDataList = Resources.LoadAll<PowerData>("Powers").ToList();
        for(int i = 0;  i < PowerDataList.Count;i++)
        {
            //PowerDataList[i].Reset();
            PowerDataList[i] = Instantiate(PowerDataList[i]);
        }

        List<CombinationData> combinationDataList = Resources.LoadAll<CombinationData>("Combinations").ToList();
        for (int i = 0; i < combinationDataList.Count; i++)
        {
            CombinationData combinationData = Instantiate(combinationDataList[i]);
            CombinationDataDictionary.Add(combinationData.Data.Name, combinationData);
        }

        List<ItemTagData> itemTagDataList = Resources.LoadAll<ItemTagData>("ItemTags").ToList();
        for (int i = 0; i < itemTagDataList.Count; i++)
        {
            ItemTagData itemTagData = Instantiate(itemTagDataList[i]);
            ItemTagDataDictionary.Add(itemTagData.Name, itemTagData);
        }
    }

    public BonusData TakeRandomBonusData(BonusData.BonusDurability bonusDurability = BonusData.BonusDurability.Run, List<BonusData> formerList = null)
    {
        // Get all possible bonus
        List<BonusData> bonusDataPool = GetBonusDataList(bonusDurability);
        List<BonusData> availableBonusDataList = new();

        for (int i = 0; i < bonusDataPool.Count; i++)
        {
            if (bonusDataPool[i] is not BD_SameFamily)
            {
                availableBonusDataList.Add(bonusDataPool[i]);
            }
            else if (SaveManager.Instance.CheckHasRunBonus<BD_SameFamily>() == null)
            {
                if (formerList == null) availableBonusDataList.Add(bonusDataPool[i]);
                else if (formerList.Find(x => x is BD_SameFamily) == null)
                {
                    availableBonusDataList.Add(bonusDataPool[i]);
                }
            }
        }

        int randomIndex = Random.Range(0, availableBonusDataList.Count);
        if (randomIndex >= availableBonusDataList.Count) return null;
        BonusData data = availableBonusDataList[randomIndex];
        return TakeRunSpecificBonus(data);
    }

    public BonusData TakeRunSpecificBonus(BonusData bonusData, BonusData.BonusDurability bonusDurability = BonusData.BonusDurability.Run)
    {
        if (RunBonusDataList.Contains(bonusData))
        {
            RunBonusDataList.Remove(bonusData);
            return bonusData;
        }
        return null;
    }

    public BonusData TakeRunBonusByName(string bonusName, BonusData.BonusDurability bonusDurability = BonusData.BonusDurability.Run)
    {
        return TakeRunSpecificBonus(RunBonusDataList.Find(x => x.name == bonusName));
    }

    public List<BonusData> GetBonusDataList(BonusData.BonusDurability bonusDurability = BonusData.BonusDurability.Run)
    {
        switch (bonusDurability)
        {
            case BonusData.BonusDurability.Run:
                return RunBonusDataList;
            case BonusData.BonusDurability.Permanent:
                return PermanentBonusDataList;
        }

        return RunBonusDataList;
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
