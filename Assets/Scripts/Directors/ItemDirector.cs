using System.Collections.Generic;
using System.Linq;
using TMPEffects.SerializedCollections;
using UnityEngine;

public class ItemDirector : MonoBehaviour
{
    #region Singleton
    public static ItemDirector Instance { get; private set; }

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

    [SerializedDictionary("Name", "BonusData")]  public SerializedDictionary<string, ItemData> ItemDataDictionary = new();
    public ItemBehavior PPPrefab;

    private void OnAwake()
    {
        List<ItemData> itemDataList = Resources.LoadAll<ItemData>("ItemData").ToList();
        for (int i = 0; i < itemDataList.Count; i++)
        {
            ItemData itemData = Instantiate(itemDataList[i]);
            ItemDataDictionary.Add($"{itemData.Save.CompleteName}", itemData);
        }
    }

    public List<ItemData> GetRandomItemDataList(int listSize)
    {
        List<ItemData> itemAvailableList = new(ItemDataDictionary.Values);
        List<ItemData> itemReturnList = new();

        for (int i = 0; i < listSize; i++)
        {
            Rarity randomRarity = GameDirector.Instance.GetRandomRarity();
            if (itemAvailableList.Count > 0)
            {
                List<ItemData> bonusRarityAvailableList = itemAvailableList.FindAll(x => x.Save.Rarity == randomRarity);
                if (bonusRarityAvailableList.Count != 0)
                {
                    ItemData newItem = bonusRarityAvailableList[Random.Range(0, bonusRarityAvailableList.Count)];
                    itemReturnList.Add(newItem);
                }
                else
                {
                    i--;
                }
            }
            else
            {
                Debug.LogError("not enough bonus to fill list");
            }
        }

        return itemReturnList;
    }

    public static ItemData GetInstantiatedItem(ItemData itemData)
    {
        return Instance.ItemDataDictionary[itemData.Save.CompleteName];
    }

    public static List<ItemData> GetInstantiatedItemList(List<ItemData> itemDataList)
    {
        List<ItemData> instantiatedItemList = new();
        for (int i = 0; i < itemDataList.Count; i++)
        {
            instantiatedItemList.Add(Instance.ItemDataDictionary[itemDataList[i].Save.CompleteName]);
        }
        return instantiatedItemList;
    }

    public static List<ItemData> GetItemDataList(List<ItemBehavior> itemScoreCalculationList)
    {
        List<ItemData> itemDataList = new();
        for (int i = 0; i < itemScoreCalculationList.Count; i++)
        {
            itemDataList.Add(itemScoreCalculationList[i].Item.Data);
        }

        return itemDataList;
    }

    public static List<ItemData> GetItemDataList(List<Item_ScoreCalculation> itemScoreCalculationList)
    {
        List<ItemData> itemDataList = new();
        for (int i = 0; i < itemScoreCalculationList.Count; i++)
        {
            itemDataList.Add(itemScoreCalculationList[i].ItemInstance.Data);
        }

        return itemDataList;
    }
}
