using DG.Tweening;
using UnityEngine;

public class GS_Reward : GameState
{
    private bool _whileTransition;

    public override void EnterState()
    {
        base.EnterState();

        GameManager.Instance.Lever.SetActive(false);

        // get calculation of how many pp should be instantiated as a reward
        // instantiate all pps
        // let the player click the button to continue their run
        GameManager.Instance.ItemManager.SpawnRewards();
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

        _whileTransition = true;
        if (GameManager.Instance.ItemManager.ItemList.Count > 0)
        {
            for (int i = 0; i < GameManager.Instance.ItemManager.ItemList.Count; i++)
            {
                if  (GameManager.Instance.ItemManager.ItemList[i] is ClickableBehavior clickableBehavior)
                {
                    clickableBehavior.Collect();
                }
            }
        }

        DOVirtual.DelayedCall(1.5f, () =>
        {
            SaveManager.Instance.NextNode();
        });
    }
}
