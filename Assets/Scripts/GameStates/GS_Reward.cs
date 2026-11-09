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

        GameManager.Instance.UIManager.Timer.gameObject.SetActive(false);

        SaveManager.Instance.CurrentReward.SpawnReward();
        List<ItemBehavior> bonusItemBehavior = GameManager.Instance.ItemManager.ItemList.FindAll(x => x is CB_Bonus);

        if (bonusItemBehavior.Count > 0)
        {
            List<BonusData> bonusDataList = BonusDirector.Instance.GetRandomBonusRunList(3, true);

            for (int i = 0; i < bonusItemBehavior.Count; i++)
            {
                CB_Bonus bonus = (CB_Bonus)bonusItemBehavior[i];
                if (bonus)
                {
                    _bonusItemBehavior.Add(bonus);
                    bonus.Setup(bonusDataList[i]);
                }
            }

            //BonusHandManager.Instance.Show();
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ExitState() 
    { 
        base.ExitState(); 
    }

    public void FinishCollect()
    {
        DOVirtual.DelayedCall(.7f, () =>
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
