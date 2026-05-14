using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class GS_Reward : GameState
{
    private bool _whileTransition;
    private List<CB_Bonus> _bonusItemBehavior = new();

    public override void EnterState()
    {
        base.EnterState();

        GameManager.Instance.Lever.SetActive(false);

        SaveManager.Instance.CurrentReward.SpawnReward();
        List<BonusData> bonusDataList = BonusDirector.Instance.GetRandomBonusRunList(3, true);
        List<ItemBehavior> bonusItemBehavior = GameManager.Instance.ItemManager.ItemList.FindAll(x => x is CB_Bonus);
        for (int i = 0; i < bonusItemBehavior.Count; i++)
        {
            CB_Bonus bonus = (CB_Bonus) bonusItemBehavior[i];
            if (bonus)
            {
                _bonusItemBehavior.Add(bonus);
                bonus.Setup(bonusDataList[i]);
            }
        }

        GameManager.Instance.UIManager.RewardMenu.OpenMenu();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ExitState() 
    { 
        base.ExitState(); 
    }

    public void CollectAndNextNode()
    {
        if (_whileTransition) return;

        if (_bonusItemBehavior.Count > 0) return;

        _whileTransition = true;
        for (int i = 0; i < GameManager.Instance.ItemManager.ItemList.Count; i++)
        {
            ItemBehavior itemBehavior = GameManager.Instance.ItemManager.ItemList[i];
            if (itemBehavior is ClickableBehavior)
            {
                ClickableBehavior clickableBehavior = (ClickableBehavior) itemBehavior;
                if (clickableBehavior is not CB_Bonus) clickableBehavior.Collect();
            }
        }

        DOVirtual.DelayedCall(1.5f, () =>
        {
            SaveManager.Instance.NextNode();
        });
    }

    public void ClearBonus(CB_Bonus bonusItemBehavior)
    {
        _bonusItemBehavior.Remove(bonusItemBehavior);
        ReleaseBonusList();
    }

    public void ReleaseBonusList()
    {
        for (int i = 0; i < _bonusItemBehavior.Count; i++)
        {
            _bonusItemBehavior[i].DestroyItem();
        }

        _bonusItemBehavior.Clear();
    }
}
