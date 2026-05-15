using System.Collections.Generic;
using System.Linq;
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

    public Dictionary<string, ItemData> ItemDataDictionary = new();

    private void OnAwake()
    {
        List<ItemData> itemDataList = Resources.LoadAll<ItemData>("ItemData").ToList();
        for (int i = 0; i < itemDataList.Count; i++)
        {
            ItemData itemData = Instantiate(itemDataList[i]);
            ItemDataDictionary.Add(itemData.Save.Name, itemData);
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
}
