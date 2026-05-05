using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "MND_Scavenge_Possession", menuName = "Scriptable Objects/MapNode/MND_Scavenge_Possession")]
public class MND_Scavenge_Possession : MND_Scavenge_Classic
{
    public int SecondPhaseGoalScore;
    public int ThirdPhaseGoalScore;

    public BossPhase BossPhase;

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

        switch (BossPhase)
        {
            case BossPhase.StartFirstPhase:
            case BossPhase.FirstPhase:
                break;
            case BossPhase.StartSecondPhase:
                GameManager.Instance.BossLock.SetLock();
                BossPhase = BossPhase.SecondPhase;
                break;
            case BossPhase.SecondPhase:
                break;
            case BossPhase.StartThirdPhase:
                GameManager.Instance.BossLock.SetLock();
                BossPhase = BossPhase.ThirdPhase;
                break;
            case BossPhase.ThirdPhase:
                break;
        }
    }

    public void BeatScore()
    {
        switch (BossPhase)
        {
            case BossPhase.StartFirstPhase:
            case BossPhase.FirstPhase:
                GameManager.Instance.GoalScore = SecondPhaseGoalScore;
                GameManager.Instance.UIManager.ScoreTextValue.SetTextValue($"{GameManager.Instance.CurrentScore} / {GameManager.Instance.GoalScore}");
                GameManager.Instance.UIManager.ScoreBarValue.SetBarValue(GameManager.Instance.CurrentScore, GameManager.Instance.GoalScore);
                BossPhase = BossPhase.StartSecondPhase;
                GameManager.Instance.UIManager.BagMenu.AllowContinue();
                break;
            case BossPhase.StartSecondPhase:
            case BossPhase.SecondPhase:
                GameManager.Instance.GoalScore = ThirdPhaseGoalScore;
                GameManager.Instance.UIManager.ScoreTextValue.SetTextValue($"{GameManager.Instance.CurrentScore} / {GameManager.Instance.GoalScore}");
                GameManager.Instance.UIManager.ScoreBarValue.SetBarValue(GameManager.Instance.CurrentScore, GameManager.Instance.GoalScore);
                BossPhase = BossPhase.StartThirdPhase;
                GameManager.Instance.UIManager.BagMenu.AllowContinue();
                break;
            case BossPhase.StartThirdPhase:
            case BossPhase.ThirdPhase:
                GameManager.Instance.SetGameState(GameManager.Instance.WinState);
                break;
        }
    }
}

public enum BossPhase
{
    StartFirstPhase = 0,
    FirstPhase = 1,
    StartSecondPhase = 2,
    SecondPhase = 3,
    StartThirdPhase = 4,
    ThirdPhase = 5
}