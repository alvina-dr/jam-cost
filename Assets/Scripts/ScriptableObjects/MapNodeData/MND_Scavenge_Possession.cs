using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[CreateAssetMenu(fileName = "MND_Scavenge_Possession", menuName = "Scriptable Objects/MapNode/MND_Scavenge_Possession")]
public class MND_Scavenge_Possession : MND_Scavenge_Classic
{
    public int SecondPhaseGoalScore;
    public int ThirdPhaseGoalScore;
    public GameObject BossPrefab;

    public BossPhase BossPhase;

    public override void SpawnItems()
    {
        ItemManager itemManager = GameManager.Instance.ItemManager;

        List<ItemData> itemDataList = ItemDirector.Instance.GetRandomItemDataList(SpawnItemParameters.ItemNumber);
        for (int i = 0; i < itemDataList.Count; i++)
        {
            ItemBehavior itemBehavior = Instantiate(itemDataList[i].Prefab);
            itemBehavior.Setup(itemDataList[i]); // actualize item with instantiated item data
            itemBehavior.transform.position = new Vector3(Random.Range(-itemManager.SpawnZone.x / 2 + itemManager.Offset.x, itemManager.SpawnZone.x / 2 + itemManager.Offset.x), Random.Range(-itemManager.SpawnZone.y / 2 + itemManager.Offset.y, itemManager.SpawnZone.y / 2 + itemManager.Offset.y), i * -0.001f);
            itemBehavior.transform.eulerAngles = new Vector3(0, 0, Random.Range(-70, 70));
            itemManager.ItemList.Add(itemBehavior);
            itemBehavior.SetSortingOrder((i * 2) + 1);
            itemManager.TopLayer = (i * 2) + 1;
        }
    }

    public override void RerollCrateEnd()
    {
        DialogueManager.Instance.DialogueRunner.StartDialogue("BossPossession_RerollCrate");
    }

    [YarnCommand("VomitOnItems")]
    public static void VomitOnItems()
    {
        Sequence sequence = Sequence.Create(useUnscaledTime:true);
        for (int i = GameManager.Instance.ItemManager.ItemList.Count - 1; i >= 0; i--)
        {
            if (i > Mathf.RoundToInt((float)SaveManager.Instance.GetScavengeNode().SpawnItemParameters.ItemNumber / 2.0f))
            {
                ItemBehavior itemBehavior = GameManager.Instance.ItemManager.ItemList[i];
                sequence.ChainCallback(() => itemBehavior.SetTag(DataLoader.Instance.GetRandomItemTagData()));
                sequence.ChainDelay(.01f);
            }
        }
    }

    public override void NewRound()
    {
        base.NewRound();

        switch (BossPhase)
        {
            case BossPhase.StartFirstPhase:
                DialogueManager.Instance.DialogueRunner.StartDialogue("BossPossession_Phase1");
                break;
            case BossPhase.FirstPhase:
                break;
            case BossPhase.StartSecondPhase:
                GameManager.Instance.BossLock.SetLock();
                BossBehavior_Possession.Instance.LockDepositBox();
                BossPhase = BossPhase.SecondPhase;
                DialogueManager.Instance.DialogueRunner.StartDialogue("BossPossession_Phase2");
                break;
            case BossPhase.SecondPhase:
                break;
        }
    }

    public override void ExitScoreCount()
    {
        switch (BossPhase)
        {
            case BossPhase.StartFirstPhase:
                break;
            case BossPhase.FirstPhase:
                break;
            case BossPhase.StartSecondPhase:
                BossBehavior_Possession.Instance.ShowBossPhaseScreen();
                break;
            case BossPhase.SecondPhase:
                break;
        }
    }

    public void BeatScore()
    {

    }

    public override void Victory()
    {
        switch (BossPhase)
        {
            case BossPhase.StartFirstPhase:
            case BossPhase.FirstPhase:
                GameManager.Instance.GoalScore = SecondPhaseGoalScore;
                GameManager.Instance.CurrentScore = 0;
                GameManager.Instance.UIManager.ScoreTextValue.SetTextValue($"{GameManager.Instance.CurrentScore} / {GameManager.Instance.GoalScore}");
                GameManager.Instance.UIManager.ScoreBarValue.SetBarValue(GameManager.Instance.CurrentScore, GameManager.Instance.GoalScore);
                BossPhase = BossPhase.StartSecondPhase;
                GameManager.Instance.UIManager.BagMenu.AllowContinue();
                break;
            case BossPhase.StartSecondPhase:
            case BossPhase.SecondPhase:
                DialogueManager.Instance.EndDialogueEvent += SetWinState;
                DialogueManager.Instance.DialogueRunner.StartDialogue("BossPossession_Victory");
                break;
        }
    }

    public override void Defeat()
    {
        DialogueManager.Instance.EndDialogueEvent += SetGameOverState;
        DialogueManager.Instance.DialogueRunner.StartDialogue("BossPossession_Defeat");
    }

    private void SetWinState()
    {
        GameManager.Instance.SetGameState(GameManager.Instance.WinState);
    }

    private void SetGameOverState()
    {
        GameManager.Instance.SetGameState(GameManager.Instance.GameOverState);
    }
}

public enum BossPhase
{
    StartFirstPhase = 0,
    FirstPhase = 1,
    StartSecondPhase = 2,
    SecondPhase = 3
}