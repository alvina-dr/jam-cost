using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class GS_Scavenging : GameState
{
    [ReadOnly] public float Timer;
    [SerializeField] private float _roundTime;
    public List<ItemBehavior> SelectedItemList = new();
    [SerializeField] private UI_TextValue _itemNumberText;

    public override void EnterState()
    {
        base.EnterState();
        GameManager.Instance.CurrentRound++;
        ResetTimer();
        AudioManager.Instance.StartClockSound();
        GameManager.Instance.ScavengingState.UpdateItemNumberText();
        GameManager.Instance.UIManager.RoundRemaining.SetTextValue(GameManager.Instance.CurrentRound + "/" + SaveManager.Instance.GetScavengeNode().RoundNumber);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Timer -= Time.deltaTime;
        if (GameManager.Instance.UIManager.Timer.GetTextValue() != Mathf.RoundToInt(Timer).ToString())
        {
            GameManager.Instance.UIManager.Timer.SetTextValue(Mathf.RoundToInt(Timer).ToString());
        }

        if (Timer <= 0)
        {
            EndOfRound();
        }

        switch (SaveManager.Instance.CurrentMapNode)
        {
            case MND_Scavenge_Empty scavengeEmptyNode:
                if (GameManager.Instance.ItemManager.ItemList.Count == 0)
                {
                    GameManager.Instance.SetGameState(GameManager.Instance.WinState);
                    // win this game
                }
                break;
        }


    }

    public void EndOfRound()
    {
        switch (SaveManager.Instance.CurrentMapNode)
        {
            case MND_Scavenge_Empty scavengeEmptyNode:
                if (GameManager.Instance.ItemManager.ItemList.Count > 0)
                {
                    GameManager.Instance.SetGameState(GameManager.Instance.GameOverState);
                }
                return;
        }

        // normal classic state
        ResetTimer();
        GameManager.Instance.SetGameState(GameManager.Instance.BagState);
    }

    public void ResetTimer()
    {
        Timer = GetRoundTime();
    }

    public float GetRoundTime()
    {
        switch (SaveManager.Instance.CurrentMapNode) 
        {
            case MND_Scavenge_Empty scavengeEmptyNode :
                return scavengeEmptyNode.Timer;
        }

        return _roundTime + SaveManager.CurrentSave.PermanentRoundBonusTime + SaveManager.CurrentSave.CurrentRun.RunRoundBonusTime;
    }

    public bool TryAddItemToSelectedList(ItemBehavior itemBehavior)
    {
        if (SelectedItemList.Count + 1 > GameManager.Instance.GetDepotSize()) return false;

        SelectedItemList.Add(itemBehavior);
        UpdateItemNumberText();
        return true;
    }

    public void RemoveItemFromSelectedList(ItemBehavior itemBehavior)
    {
        if (!SelectedItemList.Contains(itemBehavior)) return;
        SelectedItemList.Remove(itemBehavior);
    }

    public List<ItemData> GetItemDataList()
    {
        List<ItemData> itemDataList = new();
        for (int i = 0; i < SelectedItemList.Count; i++)
        {
            itemDataList.Add(SelectedItemList[i].Data);
        }
        return itemDataList;
    }

    public void CleanItemDataList()
    {
        for (int i = SelectedItemList.Count - 1; i >= 0; i--)
        {
            Destroy(SelectedItemList[i].gameObject);
        }
        SelectedItemList.Clear();
    }

    public void UpdateItemNumberText()
    {
        _itemNumberText.SetTextValue(SelectedItemList.Count + "/" + GameManager.Instance.GetDepotSize());
    }
}
