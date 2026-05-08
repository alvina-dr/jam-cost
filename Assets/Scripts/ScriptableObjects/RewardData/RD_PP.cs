using UnityEngine;

[CreateAssetMenu(fileName = "RD_PP", menuName = "Scriptable Objects/RewardData/RD_PP")]
public class RD_PP : RewardData
{
    public ItemData ItemData;
    public int PPNumber;

    public override void SpawnReward()
    {
        GameManager.Instance.ItemManager.CleanItems();

        int ppNumber = PPNumber;
        ppNumber += SaveManager.CurrentSave.EveryNodeLootPP;
        Vector2 spawnZone = GameManager.Instance.ItemManager.SpawnZone;
        Vector2 offset = GameManager.Instance.ItemManager.Offset;

        for (int i = 0; i < ppNumber; i++)
        {
            ItemBehavior itemBehavior = Instantiate(ItemData.Prefab);
            itemBehavior.Item.Data = ItemData; // actualize item with instantiated item data
            itemBehavior.transform.position = new Vector3(Random.Range(-spawnZone.x / 2 + offset.x, spawnZone.x / 2 + offset.x), Random.Range(-spawnZone.y / 2 + offset.y, spawnZone.y / 2 + offset.y), i * -0.001f);
            itemBehavior.transform.eulerAngles = new Vector3(0, 0, Random.Range(-70, 70));
            GameManager.Instance.ItemManager.ItemList.Add(itemBehavior);
            itemBehavior.SetSortingOrder((i * 2) + 1);
            GameManager.Instance.ItemManager.TopLayer = (i * 2) + 1;
        }
    }
}
