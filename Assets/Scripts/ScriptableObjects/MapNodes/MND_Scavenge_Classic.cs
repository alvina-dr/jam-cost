using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MND_Scavenge_Classic", menuName = "Scriptable Objects/MapNode/MND_Scavenge_Classic")]
public class MND_Scavenge_Classic : MapNodeData
{
    public int ScoreGoal;
    public int RoundNumber;
    public SpawnItemParameters SpawnItemParameters;
    public List<RewardData> RewardDataList;
    public RewardData SelectedRewardData;

    public virtual void SpawnItems()
    {
        ItemManager itemManager = GameManager.Instance.ItemManager;
        itemManager.CalculateTotalSpawnChance();

        for (int i = 0; i < SaveManager.Instance.GetScavengeNode().SpawnItemParameters.ItemNumber; i++)
        {
            ItemData dataItem = itemManager.GetRandomItem();
            ItemBehavior itemBehavior = Instantiate(dataItem.Prefab);
            itemBehavior.Setup(dataItem); // actualize item with instantiated item data

            int random = Random.Range(0, 2);
            itemBehavior.transform.position = new Vector3(Random.Range(-itemManager.SpawnZone.x / 2 + itemManager.Offset.x, itemManager.SpawnZone.x / 2 + itemManager.Offset.x), Random.Range(-itemManager.SpawnZone.y / 2 + itemManager.Offset.y, itemManager.SpawnZone.y / 2 + itemManager.Offset.y), i * -0.001f);
            itemBehavior.transform.eulerAngles = new Vector3(0, 0, Random.Range(-70, 70));
            itemManager.ItemList.Add(itemBehavior);
            itemBehavior.SetSortingOrder((i * 2) + 1);
            itemManager.TopLayer = (i * 2) + 1;
        }
    }

    public virtual void NewRound()
    {

    }

    public void SetRandomReward()
    {
        SelectedRewardData = RewardDataList[Random.Range(0, RewardDataList.Count)];
    }
}
