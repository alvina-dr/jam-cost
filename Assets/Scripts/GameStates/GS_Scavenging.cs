using DG.Tweening;
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

    private bool _fireDown;

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

        if (SaveManager.CurrentSave.GameFirstTimeRoundPlayed)
        {
            if (!SaveManager.CurrentSave.GameSecondTime)
            {
                SaveManager.CurrentSave.GameSecondTime = true;
                DialogueManager.Instance.EndDialogueEvent += PlayAgain;
                Time.timeScale = 0;
                DialogueManager.Instance.DialogueRunner.StartDialogue("Onboarding_GameScene_2");
            }
            else if (!SaveManager.CurrentSave.GameThirdTime)
            {
                SaveManager.CurrentSave.GameThirdTime = true;
                DialogueManager.Instance.EndDialogueEvent += PlayAgain;
                Time.timeScale = 0;
                DialogueManager.Instance.DialogueRunner.StartDialogue("Onboarding_GameScene_3");
            }
        }

    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (Input.GetKeyDown(KeyCode.U))
        {
            Timer = 0;
        }

        Timer -= Time.deltaTime;
        if (GameManager.Instance.UIManager.Timer.GetTextValue() != Mathf.RoundToInt(Timer).ToString())
        {
            GameManager.Instance.UIManager.Timer.SetTextValue(Mathf.RoundToInt(Timer).ToString());
            if (Timer <= 5)
            {
                GameManager.Instance.UIManager.Timer.SetTextColor(Color.red);
                if (!_fireDown)
                {
                    _fireDown = true;
                    GameManager.Instance.UIManager.TimerBackground.material.DOFloat(1.2f, "_FlameLevel", .3f).OnComplete(() =>
                    {
                        _fireDown = false;
                    });
                }
            }
            else
            {
                GameManager.Instance.UIManager.Timer.ResetTextColor();
                if (!_fireDown)
                {
                    _fireDown = true;
                    GameManager.Instance.UIManager.TimerBackground.material.DOFloat(0, "_FlameLevel", .3f).OnComplete(() =>
                    {
                        _fireDown = false;
                    });
                }
            }
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
        GameManager.Instance.UIManager.Timer.ResetTextColor();
        GameManager.Instance.UIManager.TimerBackground.material.SetFloat("_FlameLevel", 0);
        _fireDown = false;
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

    public List<ItemInstance> GetItemInstanceList()
    {
        List<ItemInstance> itemDataList = new();
        for (int i = 0; i < SelectedItemList.Count; i++)
        {
            itemDataList.Add(SelectedItemList[i].Item);
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

    public void PlayAgain()
    {
        Time.timeScale = 1;
    }
}
