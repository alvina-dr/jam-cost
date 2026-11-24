using System.Collections.Generic;
using System.Linq;
using TMPEffects.SerializedCollections;
using UnityEngine;

public class ItemTagDirector : MonoBehaviour
{
    #region Singleton
    public static ItemTagDirector Instance { get; private set; }

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

    [SerializedDictionary("Name", "ItemTagData")] public SerializedDictionary<string, ItemTagData> ItemTagDataDictionary = new();

    private void OnAwake()
    {
        List<ItemTagData> itemTagDataList = Resources.LoadAll<ItemTagData>("ItemTagData").ToList();
        for (int i = 0; i < itemTagDataList.Count; i++)
        {
            ItemTagDataDictionary.Add(itemTagDataList[i].Name, itemTagDataList[i]);
        }
    }

    public ItemTagData GetRandomItemTagData()
    {
        return ItemTagDataDictionary.ElementAt(Random.Range(0, ItemTagDataDictionary.Count)).Value;
    }
}
