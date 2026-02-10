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
    public List<BonusData> BonusDataList;
    public List<MapNodeData> MapNodeDataList;

    private void OnAwake()
    {
        ItemDataList = Resources.LoadAll<ItemData>("Items").ToList();
        for (int i = 0; i < ItemDataList.Count; i++)
        {
            ItemDataList[i] = Instantiate(ItemDataList[i]);
        }
        BonusDataList = Resources.LoadAll<BonusData>("Bonus").ToList();
        BonusDataList = BonusDataList.FindAll(x => x.IsAvailableInGame);
        MapNodeDataList = Resources.LoadAll<MapNodeData>("MapNodes").ToList();
        MapNodeDataList = MapNodeDataList.FindAll(x => x.IsAvailableInGame);
    }

    public BonusData TakeRandomBonusData()
    {
        // Get all possible bonus
        List<BonusData> availableBonusDataList = new();

        for (int i = 0; i < BonusDataList.Count; i++)
        {
            if (BonusDataList[i].RequiredBonusList.Count > 0)
            {
                bool hasAllBonus = true;
                for (int j = 0; j < BonusDataList[i].RequiredBonusList.Count; j++)
                {
                    if (!SaveManager.CurrentSave.CurrentRun.CurrentRunBonusList.Find(x => x == BonusDataList[i].RequiredBonusList[j]))
                    {
                        hasAllBonus = false;
                        break;
                    }
                }
                if (hasAllBonus) availableBonusDataList.Add(BonusDataList[i]);
            }
            else
            {
                availableBonusDataList.Add(BonusDataList[i]);
            }
        }

        //string debug = "";
        //for (int i = 0; i < availableBonusDataList.Count; i++)
        //{
        //    debug += availableBonusDataList[i].Name + "\n";
        //}
        //Debug.Log(debug);

        int randomIndex = Random.Range(0, availableBonusDataList.Count);
        if (randomIndex >= availableBonusDataList.Count) return null;
        BonusData data = availableBonusDataList[randomIndex];
        return TakeSpecificBonus(data);
    }

    public BonusData TakeSpecificBonus(BonusData bonusData)
    {
        if (BonusDataList.Contains(bonusData))
        {
            BonusDataList.Remove(bonusData);
            return bonusData;
        }
        return null;
    }

    public BonusData TakeBonusByName(string bonusName)
    {
        return TakeSpecificBonus(BonusDataList.Find(x => x.name == bonusName));
    }
}
