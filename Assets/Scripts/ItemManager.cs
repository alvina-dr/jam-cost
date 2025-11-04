using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<ItemBehavior> ItemList = new();
    public GenerationParameters GenerationParameters;
    [SerializeField] private Vector2 _spawnZone;
    [SerializeField] private Vector2 _offset;
    [SerializeField] public int TopLayer;

    [ReadOnly] private int _totalSpawnChance;

    [Button]
    public void ResetDumpster()
    {
        CleanItems();
        SpawnItems();
    }

    public void CleanItems()
    {
        for (int i = ItemList.Count - 1; i >= 0; i--)
        {
            Destroy(ItemList[i].gameObject);
        }

        ItemList.Clear();
    }

    public void SpawnItems()
    {
        CalculateTotalSpawnChance();
        

        for (int i = 0; i < GenerationParameters.ItemNumber; i++)
        {
            ItemData dataItem = GetRandomItem();
            //ItemData data = DataLoader.Instance.GetRandomItemData();
            ItemBehavior itemBehavior = Instantiate(dataItem.Prefab);
            itemBehavior.transform.position = new Vector3(Random.Range(-_spawnZone.x/2 + _offset.x, _spawnZone.x / 2 + _offset.x), Random.Range(-_spawnZone.y / 2 + _offset.y, _spawnZone.y / 2 + _offset.y), i * -0.001f);
            itemBehavior.transform.eulerAngles = new Vector3(0, 0, Random.Range(-70, 70));
            ItemList.Add(itemBehavior);
            itemBehavior.SetSortingOrder((i * 2) + 1);
            TopLayer = (i * 2) + 1;
        }
    }

    public ItemData GetRandomItem()
    {
        int index = Random.Range(0, _totalSpawnChance);
        for (int j = 0; j < DataLoader.Instance.ItemDataList.Count; j++)
        {
            ItemData data = DataLoader.Instance.ItemDataList[j];
            SpawnItemParameters.ItemProbability proba = SaveManager.Instance.GetClassicScavengeNode().SpawnItemParameters.GetMatchingItemData(data);

            if (index < proba.Weight)
                return DataLoader.Instance.ItemDataList[j];
            index -= proba.Weight;
        }
        return null;
    }

    [Button]
    public void CalculateTotalSpawnChance()
    {
        _totalSpawnChance = 0;
        for (int i = 0; i < SaveManager.Instance.GetClassicScavengeNode().SpawnItemParameters.ItemProbabilityList.Count; i++)
        {
            _totalSpawnChance += SaveManager.Instance.GetClassicScavengeNode().SpawnItemParameters.ItemProbabilityList[i].Weight;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, .5f); 

        Gizmos.DrawCube(_offset, new Vector3(_spawnZone.x, _spawnZone.y, 1));
    }
}
