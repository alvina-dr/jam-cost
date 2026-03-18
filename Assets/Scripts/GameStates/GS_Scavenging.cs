using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GS_Scavenging : GameState
{
    [ReadOnly] public float Timer;
    [SerializeField] private float _roundTime;
    public List<ItemBehavior> SelectedItemList = new();
    [SerializeField] private UI_TextValue _itemNumberTextValue;
    [SerializeField] private TextMeshProUGUI _itemNumberText;
    [SerializeField] private SpriteRenderer _depotSprite;

    public enum Scavenging_SubState
    {
        Scavenging = 0,
        RerollCrateAnim = 1
    }

    public Scavenging_SubState CurrentSubState;

    public override void EnterState()
    {
        base.EnterState();
        GameManager.Instance.Lever.SetActive(true);
        GameManager.Instance.CurrentRound++;
        ResetTimer();
        AudioManager.Instance.StartClockSound();
        GameManager.Instance.UIManager.RoundRemaining.SetTextValue($"Round {GameManager.Instance.CurrentRound} / {GameManager.Instance.GetMaxRoundNumber()}");
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
        UpdateItemNumberText();
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
        if (SelectedItemList.Count >= GameManager.Instance.GetDepotSize())
        {
            _itemNumberText.color = Color.red;
            _depotSprite.color = new Color32(213, 213, 213, 255);
        }
        else
        {
            _itemNumberText.color = new Color32(237, 223, 205, 255);
            _depotSprite.color = Color.white;
        }

        _itemNumberTextValue.SetTextValue(SelectedItemList.Count + "/" + GameManager.Instance.GetDepotSize());
    }
}
