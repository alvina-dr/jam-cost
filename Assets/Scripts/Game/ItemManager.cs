using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<ItemBehavior> ItemList = new();
    [SerializeField] private Vector2 _spawnZone;
    public Vector2 SpawnZone => _spawnZone;
    [SerializeField] private Vector2 _offset;
    public Vector2 Offset => _offset;

    [SerializeField] public int TopLayer;

    [ReadOnly] private int _totalSpawnChance;

    [Button]
    public void ResetDumpster()
    {
        CleanItems();
        SaveManager.Instance.GetScavengeNode().SpawnItems();
    }

    public void CleanItems()
    {
        for (int i = ItemList.Count - 1; i >= 0; i--)
        {
            Destroy(ItemList[i].gameObject);
        }

        ItemList.Clear();
    }

    public ItemData GetRandomItem()
    {
        int index = Random.Range(0, _totalSpawnChance);
        for (int j = 0; j < DataLoader.Instance.ItemDataList.Count; j++)
        {
            ItemData data = DataLoader.Instance.ItemDataList[j];
            SpawnItemParameters.ItemProbability proba = SaveManager.Instance.GetScavengeNode().SpawnItemParameters.GetMatchingItemData(data);

            if (proba != null)
            {
                if (index < proba.Weight)
                    return DataLoader.Instance.ItemDataList[j];
                index -= proba.Weight;
            }
        }
        return null;
    }

    public void CalculateTotalSpawnChance()
    {
        _totalSpawnChance = 0;
        for (int i = 0; i < SaveManager.Instance.GetScavengeNode().SpawnItemParameters.ItemProbabilityList.Count; i++)
        {
            _totalSpawnChance += SaveManager.Instance.GetScavengeNode().SpawnItemParameters.ItemProbabilityList[i].Weight;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, .5f); 

        Gizmos.DrawCube(_offset, new Vector3(_spawnZone.x, _spawnZone.y, 1));
    }
}
