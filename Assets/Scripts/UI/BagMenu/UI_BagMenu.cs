using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BagMenu : MonoBehaviour
{
    [SerializeField] private UI_Menu _menu;
    [SerializeField] private List<UI_BagSlot> _bagSlotList;
    [SerializeField] private List<UI_BagSlot> _choiceSlotList;
    public Transform DraggedItem;
    [SerializeField] private Button _confirm;
    [SerializeField] private Button _continue;

    public UI_ValueBar _scoreBar;
    public UI_TextValue _scoreGoalText;
    public UI_TextValue _currentScoreText;

    [Header("Score colors")]
    [SerializeField] private Color _addColor;
    [SerializeField] private Color _multiplyColor;
    public void OpenMenu()
    {
        _confirm.gameObject.SetActive(true);
        _continue.gameObject.SetActive(false);

        _menu.OpenMenu();

        GameManager.Instance.UIManager.TicketMenu.ResetTicket();

        _scoreBar.SetBarValue(GameManager.Instance.CurrentScore, SaveManager.Instance.GetScavengeNode().ScoreGoal, false);
        _scoreGoalText.SetTextValue(SaveManager.Instance.GetScavengeNode().ScoreGoal.ToString(), false);
        _currentScoreText.SetTextValue(GameManager.Instance.CurrentScore.ToString(), false);

        List<ItemData> bagItemList = GameManager.Instance.UIManager.TicketMenu.GetItemDataList();
        for (int i = 0; i < bagItemList.Count; i++)
        {
            if (i >= _bagSlotList.Count) break;
            _bagSlotList[i].CreateItem(bagItemList[i]);
        }

        for (int i = 0; i < _choiceSlotList.Count; i++)
        {
            _choiceSlotList[i].ClearSlot();
        }
    }

    public void CloseMenu()
    {
        _menu.CloseMenu();
    }

    public void Confirm()
    {
        CountScore();
        _confirm.gameObject.SetActive(false);
    }

    public void Continue()
    {
        GameManager.Instance.SetGameState(GameManager.Instance.ScavengingIntroState);
        _continue.gameObject.SetActive(false);
    }

    public List<UI_BagSlot> GetChosenItemSlotList()
    {
        List<UI_BagSlot> chosenItemSlotList = new();
        for (int i = 0; i < _choiceSlotList.Count; i++)
        {
            if (_choiceSlotList[i].CurrentBagItem != null) chosenItemSlotList.Add(_choiceSlotList[i]);
        }
        return chosenItemSlotList;
    }

    public void CountScore()
    {
        List<UI_BagSlot> chosenItemSlotList = GetChosenItemSlotList();
        Sequence countAnimation = DOTween.Sequence();

        for (int i = 0; i < _bagSlotList.Count; i++)
        {
            int index = i;
            if (_bagSlotList[i].CurrentBagItem != null)
            {
                countAnimation.AppendCallback(() =>
                {
                    _bagSlotList[index].ClearSlot();
                });
                countAnimation.AppendInterval(.1f);
            }
        }

        // calculate how many object there is of each family
        List<int> familyCountList = new();
        for (int i = 0; i < Enum.GetNames(typeof(ItemData.ItemFamily)).Length; i++)
        {
            familyCountList.Add(chosenItemSlotList.FindAll(x => x.CurrentBagItem.Data.Family == (ItemData.ItemFamily)i).Count);
        }

        for (int i = 0; i < chosenItemSlotList.Count; i++)
        {
            int index = i;
            int score = chosenItemSlotList[index].CurrentBagItem.Data.Price;
            //countAnimation.AppendCallback(() => AudioManager.Instance.PlaySFXSound(_countTicketEntryMoney));
            //countAnimation.AppendCallback(() => chosenItemSlotList[index].BumpPrice());
            countAnimation.Join(chosenItemSlotList[index].transform.DOShakeRotation(.3f, .3f));

            int cloneNumber = chosenItemSlotList.FindAll(x => x.CurrentBagItem.Data == chosenItemSlotList[i].CurrentBagItem.Data).Count;
            float delay = 0;

            // Basic score
            if (familyCountList[(int)chosenItemSlotList[index].CurrentBagItem.Data.Family] > 1 || cloneNumber > 1)
            {
                if (chosenItemSlotList[index].CurrentBagItem.Data.Price > 0) // if not garbage
                {
                    countAnimation.AppendCallback((TweenCallback)(() =>
                    {
                        GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + chosenItemSlotList[index].CurrentBagItem.Data.Price, chosenItemSlotList[index].transform.position, _addColor, UI_TextPopper.AnimSpeed.Quick);
                    }));
                    delay += .45f;
                }
            }
            else
            {
                if (chosenItemSlotList[index].CurrentBagItem.Data.Price > 0) // if not garbage
                {
                    countAnimation.AppendCallback((TweenCallback)(() =>
                    {
                        GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + chosenItemSlotList[index].CurrentBagItem.Data.Price, chosenItemSlotList[index].transform.position, _addColor);
                    }));
                    delay += .5f;
                }
            }

            // If several in family
            if (familyCountList[(int)chosenItemSlotList[index].CurrentBagItem.Data.Family] > 1 && chosenItemSlotList[index].CurrentBagItem.Data.Price > 0)
            {
                countAnimation.AppendInterval(delay);
                delay += .3f;
                countAnimation.AppendCallback((TweenCallback)(() =>
                {
                    score += familyCountList[(int)chosenItemSlotList[index].CurrentBagItem.Data.Family];
                    GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + familyCountList[(int)chosenItemSlotList[index].CurrentBagItem.Data.Family], chosenItemSlotList[index].transform.position, _addColor);
                }));
            }

            // if several time the same
            if (cloneNumber > 1 && chosenItemSlotList[index].CurrentBagItem.Data.Price > 0)
            {
                countAnimation.AppendInterval(delay);
                delay += .3f;
                countAnimation.AppendCallback((TweenCallback)(() =>
                {
                    score *= cloneNumber;
                    GameManager.Instance.UIManager.TextPopperManager_Number.PopText("x" + cloneNumber, chosenItemSlotList[index].transform.position, _multiplyColor, UI_TextPopper.AnimSpeed.Quick);
                }));
            }

            // Bonus family type
            List<BonusData> bonusDataFamilyList = SaveManager.Instance.CurrentSave.BonusList.FindAll(x => x is BD_FamilyMultiplier familyMultiplier && familyMultiplier.FamilyBonus == chosenItemSlotList[index].CurrentBagItem.Data.Family);
            for (int j = 0; j < bonusDataFamilyList.Count; j++)
            {
                BD_FamilyMultiplier familyMultiplier = (BD_FamilyMultiplier)bonusDataFamilyList[j];
                countAnimation.AppendInterval(delay);
                delay += .3f;
                countAnimation.AppendCallback((TweenCallback)(() =>
                {
                    score = Mathf.RoundToInt(score * familyMultiplier.BonusMultiplier);
                    GameManager.Instance.UIManager.TextPopperManager_Number.PopText("x" + familyMultiplier.BonusMultiplier, chosenItemSlotList[index].transform.position, _multiplyColor, UI_TextPopper.AnimSpeed.Quick);
                    GameManager.Instance.UIManager.TextPopperManager_Info.PopText(familyMultiplier.Name, GameManager.Instance.UIManager.BonusList.GetBonusTextSpawnPoint(familyMultiplier).position, Color.black, UI_TextPopper.AnimSpeed.Quick);
                }));
            }

            countAnimation.AppendCallback(() =>
            {
                GameManager.Instance.SetCurrentScore(GameManager.Instance.CurrentScore + score);
                _scoreBar.SetBarValue(GameManager.Instance.CurrentScore, SaveManager.Instance.GetScavengeNode().ScoreGoal);
                _currentScoreText.SetTextValue(GameManager.Instance.CurrentScore.ToString());
            });
            //countAnimation.Append(_totalScoreText.transform.DOShakePosition(.2f, 10));
            countAnimation.AppendInterval(.8f);
            //chosenItemSlotList.Add(_ticketEntryList[index].Data);
        }
        countAnimation.AppendInterval(1f);
        //countAnimation.AppendCallback(() => AudioManager.Instance.PlaySFXSound(_validateTicketSound));
        //countAnimation.AppendCallback(() => ResetTicket());
        //countAnimation.AppendInterval(1f);
        //countAnimation.AppendCallback(() => GameManager.Instance.SetGameState(GameManager.Instance.ScavengingIntroState));

        // End of round
        if (GameManager.Instance.CurrentHand >= SaveManager.Instance.GetScavengeNode().RoundNumber)
        {
            countAnimation.AppendCallback(() =>
            {
                GameManager.Instance.CheckScoreHighEnough();
                GameManager.Instance.CurrentHand = 0;
            });
        }
        // Next hand
        else
        {
            countAnimation.AppendCallback(() =>
            {
                if (GameManager.Instance.CurrentScore >= SaveManager.Instance.GetScavengeNode().ScoreGoal)
                {
                    GameManager.Instance.SetGameState(GameManager.Instance.WinState);
                    GameManager.Instance.CurrentHand = 0;
                }
                else
                {
                    _continue.gameObject.SetActive(true);
                }
            });
        }
    }

#if UNITY_EDITOR
    [Button]
    private void UpdateBagSlotList()
    {
        _bagSlotList.Clear();
        foreach (UI_BagSlot child in transform.GetComponentsInChildren<UI_BagSlot>(includeInactive: true))
        {
            _bagSlotList.Add(child);
        }
    }
#endif
}
