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
            DontDestroyOnLoad(this);
            OnAwake();
        }
    }
    #endregion

    public List<ItemData> ItemDataList;
    public List<BonusData> RunBonusDataList;
    public List<BonusData> PermanentBonusDataList;
    public List<MapNodeData> MapNodeDataList;
    public List<PowerData> PowerDataList;

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
            PowerDataList[i].Reset();
            PowerDataList[i] = Instantiate(PowerDataList[i]);
        }
    }

    public BonusData TakeRandomBonusData(BonusData.BonusDurability bonusDurability = BonusData.BonusDurability.Run)
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
                availableBonusDataList.Add(bonusDataPool[i]);
            }

        }

        int randomIndex = Random.Range(0, availableBonusDataList.Count);
        if (randomIndex >= availableBonusDataList.Count) return null;
        BonusData data = availableBonusDataList[randomIndex];
        return TakeRunSpecificBonus(data);
    }

    public BonusData TakeRunSpecificBonus(BonusData bonusData, BonusData.BonusDurability bonusDurability = BonusData.BonusDurability.Run)
    {
        switch (bonusDurability)
        {
            case BonusData.BonusDurability.Run:
                if (RunBonusDataList.Contains(bonusData))
                {
                    RunBonusDataList.Remove(bonusData);
                    return bonusData;
                }
                break;

            case BonusData.BonusDurability.Permanent:
                if (PermanentBonusDataList.Contains(bonusData))
                {
                    PermanentBonusDataList.Remove(bonusData);
                    return bonusData;
                }
                break;
        }
        return null;
    }

    public BonusData TakeRunBonusByName(string bonusName, BonusData.BonusDurability bonusDurability = BonusData.BonusDurability.Run)
    {
        switch (bonusDurability)
        {
            case BonusData.BonusDurability.Run:
                return TakeRunSpecificBonus(RunBonusDataList.Find(x => x.name == bonusName), bonusDurability);
            case BonusData.BonusDurability.Permanent:
                return TakeRunSpecificBonus(PermanentBonusDataList.Find(x => x.name == bonusName), bonusDurability);
        }

        Debug.LogError("ERROR : bonus durability case not treated.");
        return null;
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
}
