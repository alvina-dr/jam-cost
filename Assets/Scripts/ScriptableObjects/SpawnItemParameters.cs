using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnItemParameters", menuName = "Scriptable Objects/SpawnItemParameters")]
public class SpawnItemParameters : ScriptableObject
{
    public List<ItemProbability> ItemProbabilityList = new();

    public ItemProbability GetMatchingItemData(ItemData itemData)
    {
        return ItemProbabilityList.Find(x => x.ItemData.Prefab == itemData.Prefab);
    }

    [System.Serializable]
    public class ItemProbability
    {
        public int Weight;
        public ItemData ItemData;
    }
}
