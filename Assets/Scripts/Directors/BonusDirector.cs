using System.Collections.Generic;
using System.Linq;
using TMPEffects.SerializedCollections;
using UnityEngine;

public class BonusDirector : MonoBehaviour
{
    #region Singleton
    public static BonusDirector Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            OnAwake();
        }
    }
    #endregion

    [SerializedDictionary("Name", "BonusData")] public SerializedDictionary<string, BonusData> RunBonusDataDictionary = new();
    [SerializedDictionary("Name", "BonusData")] public SerializedDictionary<string, BonusData> PermanentBonusDataDictionary = new();

    private void OnAwake()
    {
        List<BonusData> bonusRunDataList = Resources.LoadAll<BonusData>("BonusData/Run").ToList();
        bonusRunDataList = bonusRunDataList.FindAll(x => x.IsAvailableInGame);
        for (int i = 0; i < bonusRunDataList.Count; i++)
        {
            BonusData bonusData = Instantiate(bonusRunDataList[i]);
            bonusData.name = bonusData.Name;
            RunBonusDataDictionary.Add(bonusData.Name, bonusData);
        }

        List<BonusData> bonusPermanentDataList = Resources.LoadAll<BonusData>("BonusData/Permanent").ToList();
        bonusPermanentDataList = bonusPermanentDataList.FindAll(x => x.IsAvailableInGame);
        for (int i = 0; i < bonusPermanentDataList.Count; i++)
        {
            BonusData bonusData = Instantiate(bonusPermanentDataList[i]);
            bonusData.name = bonusData.Name;
            PermanentBonusDataDictionary.Add(bonusData.Name, bonusData);
        }
    }

    public List<BonusData> GetRandomBonusRunList(int listSize, bool excludeAlreadyInStock = false)
    {
        List<BonusData> bonusAvailableList = new(RunBonusDataDictionary.Values);
        List<BonusData> bonusReturnList = new();

        if (excludeAlreadyInStock)
        {
            for (int i = bonusAvailableList.Count - 1; i >= 0; i--)
            {
                string bonusName = bonusAvailableList[i].Name;
                BonusData bonusData = SaveManager.Instance.CurrentRunBonusList.Find(x => x.Name == bonusName);
                if (bonusData != null)
                {
                    bonusAvailableList.Remove(bonusData);
                }
            }
        }

        for (int i = 0; i < listSize; i++)
        {
            Rarity randomRarity = GameDirector.Instance.GetRandomRarity();
            if (bonusAvailableList.Count > 0)
            {
                List<BonusData> bonusRarityAvailableList = bonusAvailableList.FindAll(x => x.Rarity == randomRarity);
                if (bonusRarityAvailableList.Count != 0)
                {
                    BonusData newBonus = bonusRarityAvailableList[Random.Range(0, bonusRarityAvailableList.Count)];
                    bonusAvailableList.Remove(newBonus);
                    bonusReturnList.Add(newBonus);
                }
                else
                {
                    // not enough bonus in this rarity, make a new pick from another rarity
                    i--;
                }
            }
            else
            {
                Debug.LogError("not enough bonus to fill list");
            }
        }

        return bonusReturnList;
    }

    public BonusData GetInstantiatedRunBonusData(BonusData bonusData)
    {
        return RunBonusDataDictionary[bonusData.Name];
    }
}
