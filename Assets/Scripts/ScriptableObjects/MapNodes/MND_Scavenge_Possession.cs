using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "MND_Scavenge_Possession", menuName = "Scriptable Objects/MapNode/MND_Scavenge_Possession")]
public class MND_Scavenge_Possession : MND_Scavenge_Classic
{

    public override void SpawnItems()
    {
        ItemManager itemManager = GameManager.Instance.ItemManager;
        itemManager.CalculateTotalSpawnChance();

        for (int i = 0; i < SpawnItemParameters.ItemNumber; i++)
        {
            ItemData dataItem = itemManager.GetRandomItem();
            ItemBehavior itemBehavior = Instantiate(dataItem.Prefab);
            itemBehavior.Setup(dataItem); // actualize item with instantiated item data

            if (i > Mathf.RoundToInt((float)SpawnItemParameters.ItemNumber / 2.0f))
            {
                itemBehavior.SetTag(DataLoader.Instance.GetRandomItemTagData());
            }

            itemBehavior.transform.position = new Vector3(Random.Range(-itemManager.SpawnZone.x / 2 + itemManager.Offset.x, itemManager.SpawnZone.x / 2 + itemManager.Offset.x), Random.Range(-itemManager.SpawnZone.y / 2 + itemManager.Offset.y, itemManager.SpawnZone.y / 2 + itemManager.Offset.y), i * -0.001f);
            itemBehavior.transform.eulerAngles = new Vector3(0, 0, Random.Range(-70, 70));
            itemManager.ItemList.Add(itemBehavior);
            itemBehavior.SetSortingOrder((i * 2) + 1);
            itemManager.TopLayer = (i * 2) + 1;
        }
    }

    public override void NewRound()
    {
        base.NewRound();
        switch (GameManager.Instance.CurrentRound)
        {
            case 0:
                GameManager.Instance.BossLock.SetLock();
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }
    }
}
