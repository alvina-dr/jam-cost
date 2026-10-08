using System.Collections.Generic;
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

        float bonusSpace = .2f;
        float bonusSize = 1.5f;
        if (RewardItemData.Prefab is CB_Bonus)
        {
            for (int i = 0; i < GameManager.Instance.ItemManager.ItemList.Count; i++)
            {
                float totalSpace = bonusSpace * (GameManager.Instance.ItemManager.ItemList.Count - 1) + bonusSize * GameManager.Instance.ItemManager.ItemList.Count;
                GameManager.Instance.ItemManager.ItemList[i].transform.position = new Vector3((i * bonusSpace) + (i * bonusSize + bonusSize / 2) - totalSpace / 2, 0.45f + Random.Range(-.2f, .2f), GameManager.Instance.ItemManager.ItemList[i].transform.position.z);
                GameManager.Instance.ItemManager.ItemList[i].transform.transform.eulerAngles = new Vector3(0, 0, Random.Range(-10, 10));
            }
        }

        // PP every node
        if (SaveManager.CurrentSave.EveryNodeLootPP > 0)
        {
            for (int i = 0; i < SaveManager.CurrentSave.EveryNodeLootPP; i++)
            {
                ItemBehavior itemBehavior = Instantiate(ItemDirector.Instance.PPPrefab);
                itemBehavior.Setup(RewardItemData); // actualize item with instantiated item data
                itemBehavior.transform.position = new Vector3(Random.Range(-spawnZone.x / 2 + offset.x, spawnZone.x / 2 + offset.x), Random.Range(-spawnZone.y / 2 + offset.y, spawnZone.y / 2 + offset.y), i * -0.001f);
                itemBehavior.transform.eulerAngles = new Vector3(0, 0, Random.Range(-70, 70));
                GameManager.Instance.ItemManager.ItemList.Add(itemBehavior);
                itemBehavior.SetSortingOrder((i * 2) + 1);
                GameManager.Instance.ItemManager.TopLayer = (i * 2) + 1;
            }
        }

    }
}
