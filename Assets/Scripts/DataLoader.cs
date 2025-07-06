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
            OnAwake();
        }
    }
    #endregion

    public List<ItemData> ItemDataList;
    public List<BonusData> BonusDataList;
    
    private void OnAwake()
    {
        ItemDataList = Resources.LoadAll<ItemData>("Items").ToList();
        for (int i = 0; i < ItemDataList.Count; i++)
        {
            ItemDataList[i] = Instantiate(ItemDataList[i]);
        }
        BonusDataList = Resources.LoadAll<BonusData>("Bonus").ToList();
    }

    public BonusData TakeRandomBonusData()
    {
        int randomIndex = Random.Range(0, BonusDataList.Count);
        BonusData data = BonusDataList[randomIndex];
        BonusDataList.RemoveAt(randomIndex);
        return data;
    }

    public ItemData GetRandomItemData()
    {
        return ItemDataList[Random.Range(0, ItemDataList.Count)];
    }
}
