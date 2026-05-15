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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, .5f); 

        Gizmos.DrawCube(_offset, new Vector3(_spawnZone.x, _spawnZone.y, 1));
    }
}
