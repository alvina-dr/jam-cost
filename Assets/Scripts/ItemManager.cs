using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<ItemBehavior> ItemList = new();
    public GenerationParameters GenerationParameters;
    [SerializeField] private Vector2 _spawnZone;
    [SerializeField] private Vector2 _offset;

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
        for (int i = 0; i < GenerationParameters.ItemNumber; i++)
        {
            ItemData data = DataLoader.Instance.GetRandomItemData();
            ItemBehavior itemBehavior = Instantiate(data.Prefab);
            itemBehavior.transform.position = new Vector3(Random.Range(-_spawnZone.x/2 + _offset.x, _spawnZone.x / 2 + _offset.x), Random.Range(-_spawnZone.y / 2 + _offset.y, _spawnZone.y / 2 + _offset.y), 0);
            itemBehavior.transform.eulerAngles = new Vector3(0, 0, Random.Range(-70, 70));
            ItemList.Add(itemBehavior);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, .5f); 

        Gizmos.DrawCube(_offset, new Vector3(_spawnZone.x, _spawnZone.y, 1));
    }
}
