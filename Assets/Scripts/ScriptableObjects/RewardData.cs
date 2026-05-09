using UnityEngine;

[CreateAssetMenu(fileName = "RewardData", menuName = "Scriptable Objects/RewardData")]
public class RewardData : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;
    public ItemData RewardItemData;
    public int RewardNumber;

    public void SpawnReward()
    {
        GameManager.Instance.ItemManager.CleanItems();

        int rewardNumber = RewardNumber;
        rewardNumber += SaveManager.CurrentSave.EveryNodeLootPP;
        Vector2 spawnZone = GameManager.Instance.ItemManager.SpawnZone;
        Vector2 offset = GameManager.Instance.ItemManager.Offset;

        for (int i = 0; i < rewardNumber; i++)
        {
            ItemBehavior itemBehavior = Instantiate(RewardItemData.Prefab);
            itemBehavior.Setup(RewardItemData); // actualize item with instantiated item data
            itemBehavior.transform.position = new Vector3(Random.Range(-spawnZone.x / 2 + offset.x, spawnZone.x / 2 + offset.x), Random.Range(-spawnZone.y / 2 + offset.y, spawnZone.y / 2 + offset.y), i * -0.001f);
            itemBehavior.transform.eulerAngles = new Vector3(0, 0, Random.Range(-70, 70));
            GameManager.Instance.ItemManager.ItemList.Add(itemBehavior);
            itemBehavior.SetSortingOrder((i * 2) + 1);
            GameManager.Instance.ItemManager.TopLayer = (i * 2) + 1;
        }
    }
}
